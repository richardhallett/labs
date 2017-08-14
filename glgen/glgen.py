"""
   glgen is a generator that produces a C# GL library.
   It does this by parsing the official opengl xml spec files and then building delegate prototypes and extension loading as appropriate.
"""

__version__ = '0.1'

import re
import os
import argparse
import urllib.request
import xml.etree.ElementTree as ET
from string import Template
from io import StringIO
from collections import OrderedDict

# Type lookups
type_mapping = {
    'GLenum' : 'uint',
    'GLfloat': 'float',
    'GLint' : 'int',
    'GLsizei': 'int',
    'GLvoid': 'void',
    'GLbitfield': 'uint',
    'GLdouble': 'double',
    'GLuint': 'uint',
    'GLboolean': 'bool',
    'GLubyte': 'byte',
    'GLubyte*': 'string',
    'GLclampf': 'float',
    'GLclampd': 'double',
    'GLchar': 'byte',
    'GLcharARB': 'byte',
    'GLshort': 'short',
    'GLbyte': 'sbyte',
    'GLushort': 'ushort',
    'GLhalf': 'ushort',
    'GLuint64': 'ulong',
    'GLint64': 'long',
    'GLuint64EXT': 'ulong',
    'GLsync': 'IntPtr',
    'GLsizeiptr' : 'IntPtr',
    'GLintptr' : 'IntPtr',
    'GLhandleARB': 'uint',
    'GLvdpauSurfaceNV': 'IntPtr',
    'GLfixed': 'int',
    'GLintptrARB':  'IntPtr',
    'GLsizeiptrARB':  'IntPtr',
    'GLhalfNV': 'ushort',
    'GLDEBUGPROC': 'DebugProc',
    'GLDEBUGPROCARB': 'DebugProcArb',
    'GLDEBUGPROCAMD': 'DebugProcAmd',
    'GLDEBUGPROCKHR': 'DebugProcKhr',
    'GLeglImageOES': 'IntPtr',
    'GLint64EXT': 'long',
    'GLclampx': 'int',
    'void' : 'void',
    'struct': 'IntPtr'
}

class SpecParser:
    def __init__(self, api, profile, version=None, explicit_ext_only=False, required_extensions=[], unsafe=False):
        self.api = api
        self.profile = profile
        self.version = version
        self.explicit_ext_only=explicit_ext_only
        self.required_extensions = required_extensions
        self.unsafe = unsafe

        # I really dont get why the GL spec has "glcore", this does not match the featureset api
        # So I'm just going to "guess" it.
        if self.api == 'gl' and self.profile == 'core':
            self.extension_api = 'glcore'
        else:
            self.extension_api = api

        # Raw data parsed out, includes everything
        self.all_enums = {}
        self.all_enum_groups = []
        self.all_functions = {}

        # We keep track of all the "loaded" enums/functions, based upon requested api/profile/version
        self.enums = OrderedDict()
        self.functions = OrderedDict()

        # Specific data matching requested api/profile/version
        self.features = OrderedDict()
        self.extensions = OrderedDict()

    def parse(self, filename):
        self.filename = filename

        # Start at the top
        tree = ET.parse(filename)
        self._root = tree.getroot()

        # Get all the Enums defined
        self._parse_enums()

        # Get all the functions defined
        self._parse_functions()

        # Work out what enums/funcs are actually in feature set we are requesting
        self._parse_features()

        # Build the feature dictionarys
        for func_name, func in self.functions.items():
            feature = func['feature']
            if feature not in self.features:
                self.features[feature] = {'enums': [], 'functions': []}

            self.features[feature]['functions'].append(func)

        for enum_name, enum in self.enums.items():
            feature = enum['feature']
            if feature not in self.features:
                self.features[feature] = {'enums': [], 'functions': []}

            self.features[feature]['enums'].append(enum)

        # Work out what extensions are applicable
        self._parse_extensions()

    def _parse_extensions(self):
        for extension in self._root.iter('extension'):
            enums = []
            functions = []
            name = extension.attrib['name']

            supported = extension.attrib['supported']

            r = re.compile('^(' + supported + ')$')

            # If we've set to be explicit then we ignore everything now in the required list
            if self.explicit_ext_only and name not in self.required_extensions:
                continue

            # Skip any extensions that are not supported by our requested API unless we've asked to be explicit
            if not self.explicit_ext_only and not r.match(self.extension_api):
                continue

            for require in extension.iter('require'):

                if 'api' in require.attrib:
                    api = require.attrib['api']

                    # If this API doesn't match ours, then we need to skip
                    if api != self.api:
                        continue

                if 'profile' in require.attrib:
                    profile = require.attrib['profile']
                else:
                    profile = 'all'

                # Skip profiles that we dont want
                if profile != self.profile and profile != 'all':
                    continue

                for enum in require.iter('enum'):
                    enum_name = enum.attrib['name']

                    # Do we already have this required due to feature sets?
                    if enum_name not in self.enums:
                        enum = self.all_enums[enum_name]
                        enum['profile'] = profile
                        enums.append(enum)
                        self.enums[enum_name] = enum

                for command in require.iter('command'):
                    func_name = command.attrib['name']

                    # Do we already have this required due to feature sets?
                    if func_name not in self.functions:
                        func = self.all_functions[func_name]
                        func['profile'] = profile
                        functions.append(func)
                        self.functions[func_name] = func

            if enums or functions:
                self.extensions[name] = {'enums': enums, 'functions': functions}


    def _parse_features(self):
        for feature in self._root.iter('feature'):
            feature_name = feature.attrib['name']
            version = float(feature.attrib['number'])
            api = feature.attrib['api']

            # Skip anything that isnt the api we requested
            if api != self.api:
                continue

            # Skip anything that isnt below version we requested
            if self.version and version > self.version:
                continue

            for require in feature.iter('require'):
                # Does spec specify a profile if so use otherwise we say all
                if 'profile' in require.attrib:
                    profile = require.attrib['profile']
                else:
                    profile = 'all'

                # Skip any required feature sets that dont match the specified profile or are applicable to all
                if profile != self.profile and profile != 'all':
                    continue

                # Add in enums for api
                for enum in require.iter('enum'):
                    enum_name = enum.attrib['name']
                    # Get from global list
                    enum = self.all_enums[enum_name]
                    # Add Extra info
                    enum['version'] = version
                    enum['profile'] = profile
                    enum['feature'] = feature_name
                    # Add to list
                    self.enums[enum_name] = enum

                # Add in functions for api
                for command in require.iter('command'):
                    func_name = command.attrib['name']
                    # Get from global list
                    func = self.all_functions[func_name]
                    # Add Extra info
                    func['version'] = version
                    func['profile'] = profile
                    func['feature'] = feature_name
                    # Add to list
                    self.functions[func_name] = func

            for remove in feature.iter('remove'):
                # Does spec specify a profile if so use otherwise we say all
                if 'profile' in remove.attrib:
                    profile = remove.attrib['profile']
                else:
                    profile = 'all'

                # Specify deprecation details for enums
                for enum in remove.iter('enum'):
                    enum_name = enum.attrib['name']

                    # If we actually match the profile, we just wanna ditch it
                    if profile == self.profile:
                        if enum_name in self.enums:
                            del self.enums[enum_name]
                    else:
                        # Get from global list
                        enum = self.all_enums[enum_name]
                        # Add Extra info
                        enum['deprecatedVersion'] = version
                        enum['deprecatedProfile'] = profile

                # Specify deprecation details for functions
                for command in remove.iter('command'):
                    func_name = command.attrib['name']

                    # Get from global list
                    func = self.all_functions[func_name]
                    # Add Extra info
                    func['deprecatedVersion'] = version
                    func['deprecatedProfile'] = profile

                    # If we actually match the profile, we just wanna ditch it
                    if profile == self.profile:
                        if func_name in self.functions:
                            del self.functions[func_name]

    def _parse_functions(self):
        # Function defs
        for command in self._root.findall('./commands/command'):

            # Return type
            proto = command.find('proto')
            ptype = proto.find('ptype')
            c_return_type = ''
            if proto.text:
                c_return_type += proto.text

            if ptype is not None:
                c_return_type += ptype.text
                c_return_type += ptype.tail # The rest of the prototype

            # Figure out the marshall return type but also a better return type if we can
            return_type, better_return_type = self._parse_c_return_type(c_return_type)

            # Function Name
            func_name = proto.find('name').text

            # Arguments
            args = []
            for param in command.iter('param'):
                ptype = param.find('ptype')
                c_arg_type = ''
                if param.text:
                    c_arg_type += param.text

                if ptype is not None:
                    c_arg_type += ptype.text
                    c_arg_type += ptype.tail # The rest of the prototype

                # We use the length param to work out if we're most likely an array or not
                # The specific case atm I check is just to see if its a length of 1, i.e. not.
                # In the future probably could do something more clever.
                array = False
                if 'len' in param.attrib:
                    array = True
                    length = param.attrib['len']
                    if length == '1':
                        array = False

                # Hardcoded bugfix for http://www.khronos.org/bugzilla/show_bug.cgi?id=996
                if func_name == 'glShaderSource':
                    array = True

                arg_type = self._parse_c_arg_type(c_arg_type, array)

                # If we have a group that exists, we want to use this enum for the type
                enum_type = None
                if 'group' in param.attrib:
                    group = param.attrib['group']
                    if group in self.all_enum_groups and arg_type == 'uint':
                        enum_type = group

                arg_name = self._get_safe_name(param.find('name').text)
                arg = {'name': arg_name, 'type': arg_type, 'enum_type': enum_type}
                args.append(arg)

            function = {
                'name': func_name,
                'return_type': return_type,
                'better_return_type': better_return_type,
                'args': args,
                'version': None,
                'profile': None,
                'deprecatedProfile': None,
                'deprecatedVersion': None
            }

            self.all_functions[func_name] = function

    def _parse_enums(self):
        for enum in self._root.findall('./enums/enum'):
            name = enum.get('name')
            value = enum.get('value')
            type = self._parse_enum_type(enum.get('type'))

            # Weird hack because it seems the spec has the odd enum for extensions that do not specify a type but use negative numbers
            if value.startswith('-') and type == 'uint':
                type = 'int'

            enum = {
                'name': name,
                'group': None,
                'value': value,
                'type': type,
                'version': None,
                'profile': None,
                'deprecatedProfile': None,
                'deprecatedVersion': None
                }

            self.all_enums[name] = enum

        # Enumeration types
        for group in self._root.iter('group'):
            enum_group = group.attrib['name']
            self.all_enum_groups.append(enum_group)
            for enum in group.iter('enum'):
                enum_name = enum.attrib['name']
                if enum_name in self.all_enums:
                    self.all_enums[enum_name]['group'] = enum_group

    def _parse_enum_type(self, enum_type):
        """Take the specified GL type string for enums and get a better type out"""
        if enum_type == 'ull':
            new_type = 'ulong'
        elif enum_type == 'u':
            new_type = 'uint'
        else:
            new_type = 'uint'

        return new_type

    def _parse_c_return_type(self, c_return_type):
        m = re.search(r'(const)?\s?(\w+)\s(\*+)?', c_return_type)
        if m:
            const = m.group(1)
            return_type = m.group(2)
            pointer = m.group(3)

            if pointer:
                new_return_type = 'IntPtr'
            else:
                new_return_type = type_mapping[return_type]

            better_return_type = None
            if c_return_type == 'const GLubyte *':
                # Better suggestion
                better_return_type = 'string'

            return (new_return_type, better_return_type)

    def _parse_c_arg_type(self, c_arg_type, array=True):
        """Take the C based type and get better type back"""

        m = re.search(r'(const)?\s?(\w+)\s(\*const|\*)?(\*+)?', c_arg_type)
        if m:
            const = m.group(1)
            return_type = m.group(2)
            pointer1 = m.group(3)
            pointer2 = m.group(4)

            # Get the generic type mapping
            new_type = type_mapping[return_type]

            # We need to do additional checking to see if its actually a pointer
            if pointer1:
                if new_type == 'void':
                    new_type = 'IntPtr'
                elif const and new_type == 'byte' and array: # Special handling for strings
                    if pointer2:
                       new_type = 'string[]'
                    else:
                        new_type = 'string'
                # Else we're not saying we're a pointer but an array most likely, though the additional check passed through confirms
                elif new_type != 'IntPtr':
                    if array:
                        if not self.unsafe:
                            new_type +='[]'
                        else:
                            new_type +='*'
                    else:
                        if self.unsafe:
                            new_type +='*'
                        else:
                            new_type = 'IntPtr'

            return new_type

    def _get_safe_name(self, name):
        new_name = name
        if name in ['params', 'ref', 'string', 'event', 'object', 'base', 'in', 'out']:
            new_name = '@' + name

        return new_name


def get_gl_spec(remote_url):
    local_filename = remote_url.split('/')[-1]
    if not os.path.exists(local_filename):
        print("Downloading core file")
        core_file = urllib.request.urlopen(remote_url)
        with open(local_filename, 'wb') as f:
            f.writelines(core_file.readlines())
    else:
        print("Using the local spec file, skipping download.")

    return local_filename


def get_constants_string(enums):

    output = StringIO()

    padding = '        '
    scope = 'public const'
    for enum in enums:
        # Constants
        constant_type = enum['type']
        constant_value = enum['value']
        full_constant = scope + ' ' + constant_type + ' ' + enum['name'] + ' = (' + constant_type + ')' + constant_value

        output.write(padding + full_constant + ';\n')

    contents = output.getvalue()
    output.close()

    return contents

def get_function_string(function, group, is_extension, arguments, argument_names, unsafe=False):
    output = StringIO()
    padding = '        '

    if unsafe:
        delegate = 'internal unsafe delegate' + ' ' + function['return_type'] + ' ' + function['name'] + '(' + ', '.join(arguments) + ');'
    else:
        delegate = 'internal delegate' + ' ' + function['return_type'] + ' ' + function['name'] + '(' + ', '.join(arguments) + ');'

    delegate_instance = 'internal static ' + function['name'] + ' _' + function['name'] + ';'

    # Delegate
    output.write(padding + '[SuppressUnmanagedCodeSecurity]\n')
    output.write(padding + delegate + '\n') # The function delegate
    output.write(padding + delegate_instance) # The function delegate

    output.write('\n')

    def write_method(args, arg_names):

        if function['better_return_type']:
            return_type = function['better_return_type']
        else:
            return_type = function['return_type']

        xstr = lambda s: '' if s is None else str(s)

        version = xstr(function['version'])
        profile = xstr(function['profile'])
        deprecatedVersion = xstr(function['deprecatedVersion'])
        deprecatedProfile = xstr(function['deprecatedProfile'])
        if is_extension:
            _is_extension = "true"
        else:
            _is_extension = "false"

        versionAttrib = '[Version(Group="{0}", Version = "{1}", Profile="{2}", DeprecatedVersion="{3}", DeprecatedProfile="{4}", EntryPoint="{5}", IsExtension={6})]'.format(group, version, profile, deprecatedVersion, deprecatedProfile, function['name'], _is_extension)

        if unsafe:
            method = 'public unsafe static' + ' ' + return_type + ' ' + function['name'][2:] + '(' + ', '.join(args) + ')'
        else:
            method = 'public static' + ' ' + return_type + ' ' + function['name'][2:] + '(' + ', '.join(args) + ')'

        #Method
        output.write('\n')
        output.write(padding + versionAttrib + '\n')
        output.write(padding + method) # The function delegate
        output.write('\n')
        output.write(padding + '{\n')

        method_padding = '            '

        if function['return_type'] != 'void':
            #output.write(method_padding + '{0} data = ({0})InvokeFunction<{1}>({2});'.format(function['return_type'], function['name'], ', '.join(arg_names)))
            output.write(method_padding + '{0} data = _{1}({2});'.format(function['return_type'], function['name'], ', '.join(arg_names)))

            output.write('\n')
            # We know how to handle strings in this case so lets marshall it to something friendlier.
            # We handle it like this because we dont want the CLR cleaning up the memory for us as this is done by GL.
            if return_type == 'string':
                output.write(method_padding + 'return Marshal.PtrToStringAnsi(data);')
            else:
                output.write(method_padding + 'return data;')
        else:
            output.write(method_padding + '_{0}({1});'.format(function['name'], ', '.join(arg_names)))

        output.write('\n')
        output.write(padding + '}\n')
        output.write(padding + '\n')

    # Atm we only have one method, but we might wanna do different overloads
    write_method(arguments, argument_names)

    contents = output.getvalue()
    output.close()

    return contents

def get_functions_string(functions, group, is_extension, unsafe=False):
    output = StringIO()

    for func in functions:
        full_args = [] # All the arguments
        arg_names = [] # Just the argument names
        for arg in func['args']:
            arg_type = arg['type']
            arg_name = arg['name']

            # The full argument type + name
            full_arg = arg_type + ' ' + arg_name

            full_args.append(full_arg)
            arg_names.append(arg_name)

        full_function_string = get_function_string(func, group, is_extension, full_args, arg_names, unsafe)

        output.write(full_function_string)

    contents = output.getvalue()
    output.close()

    return contents

def write_interop(filename, namespace, features, extensions, functions, unsafe=False):

    # GL Core file
    gl_core_string = None
    padding = '        '
    with open('template.cs', 'r') as f:
        gl_core_template = Template(f.read())

        # Build the string for all the features we're writing out
        output = StringIO()
        for feature_name, feature in features.items():

            output.write(padding)
            output.write("#region " + feature_name + "\n")
            constants_string = get_constants_string(feature['enums'])
            output.write(constants_string + "\n")
            functions_string = get_functions_string(feature['functions'], feature_name, False, unsafe)
            output.write(functions_string + "\n")
            output.write(padding)
            output.write("#endregion\n")
            output.write("\n")

        features_string = output.getvalue()
        output.close()

        # Build the string for all the extensions we're writing out
        output = StringIO()
        for extension_name, extension in extensions.items():
            output.write(padding)
            output.write("#region " + extension_name + "\n")
            constants_string = get_constants_string(extension['enums'])
            output.write(constants_string + "\n")
            functions_string = get_functions_string(extension['functions'], extension_name, True, unsafe)
            output.write(functions_string + "\n")
            output.write(padding)
            output.write("#endregion\n")
            output.write("\n")

        extensions_string = output.getvalue()
        output.close()

        # Build the string for all functions we are going to want to try and load.
        output = StringIO()
        for func in functions:
            output.write('            ')
            output.write("_{0} = LoadFunction<{0}>();".format(func))
            output.write("\n")
        functions_loading_string = output.getvalue()
        output.close()

        gl_core_string = gl_core_template.safe_substitute(namespace=namespace, features=features_string, extensions=extensions_string, functions_loading=functions_loading_string)

    with open(filename, 'wb') as f:
        f.write(bytes(gl_core_string, 'UTF-8'))

if __name__ == '__main__':

    import argparse
    import sys

    parser = argparse.ArgumentParser(description='Build OpenGL bindings for C# using core spec files.')
    parser.add_argument('--api', dest='api', choices=['gl', 'gles1', 'gles2'], default='gl', help="OpenGL API")
    parser.add_argument('--profile', dest='profile', choices=['core', 'compatability'], default='core', help="OpenGL profile")
    parser.add_argument('--version', dest='version', type=float, help="OpenGL version")
    parser.add_argument('--out', dest='out', default="GL.cs", help="OpenGL output filename")
    parser.add_argument('--explicit-ext-only', dest='explicit_ext_only', action="store_true", help="Only use defined explicit definitions, don't include anything else")
    parser.add_argument('--extensions_file', dest='extensions_file', help="Full path to extensions file, specifys the required exceptions you want to load")
    parser.add_argument('--namespace', dest='namespace', default="glgen", help="The namespace to put all the code in.")
    parser.add_argument('--unsafe', dest='unsafe', default=False, help="Build unsafe versions of functions i.e. raw pointer access")

    args = parser.parse_args()

    if args.extensions_file:
        with open(args.extensions_file) as f:
            required_extensions = f.read().splitlines()
    else:
        required_extensions = []

    if args.api == 'gles1' and args.version > 1.0:
        parser.error("Oops you've tried to specify gles1 api with a version greater than 1.0")

    if args.api == 'gles2' and args.version < 2.0:
        parser.error("Oops you've tried to specify gles2 api with a version less than 2.0")

    # GL XML spec file URL.
    remote_spec_url = 'https://cvs.khronos.org/svn/repos/ogl/trunk/doc/registry/public/api/gl.xml'

    # Go get the core gl file that we want to parse
    local_spec = get_gl_spec(remote_spec_url)

    spec_parser = SpecParser(args.api, args.profile, args.version, args.explicit_ext_only, required_extensions, args.unsafe)
    spec_parser.parse(local_spec)

    # # # Write out the file
    output_filename = args.out
    write_interop(output_filename, args.namespace, spec_parser.features, spec_parser.extensions, spec_parser.functions, args.unsafe)
