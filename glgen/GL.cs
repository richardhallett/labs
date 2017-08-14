#pragma warning disable 1591
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Reflection;
using System.Collections.Generic;

IofeX2QJE3YknRllLqrZ
l

namespace glgen
{
    /// <summary>
    /// Platform independent OpenGL bindings
    /// </summary>
    public partial class GL 
    {        
        #if PLATFORM_OSX
            // TODO            
        #elif PLATFORM_LINUX
            internal const string OPENGLLIB = "libGL.so.1";

            [DllImport(OPENGLLIB)]
            public static extern IntPtr glXGetProcAddress(string name);

            public static void LoadLib()
            {
                // Not Needed
            }

            public static void UnLoadLib()
            {
                // Not Needed
            }

            public static IntPtr GetProc(string name)
            {
                IntPtr proc = glXGetProcAddress(name);
                
                return proc;
            }
        #else 
            // Default is windows

            private static IntPtr opengldll;

            internal const string OPENGL32LIB = "opengl32";
            internal const string KERNEL32LIB = "kernel32";

            [DllImport(OPENGL32LIB)]
            public static extern IntPtr wglGetProcAddress(string name);

            [DllImport(KERNEL32LIB)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string name);

            [DllImport(KERNEL32LIB)]
            public static extern IntPtr LoadLibraryA(string name);

            [DllImport(KERNEL32LIB)]
            public static extern bool FreeLibrary(IntPtr hModule);
         
            public static void LoadLib()
            {
                opengldll = LoadLibraryA("opengl32.dll");
            }

            public static void UnLoadLib()
            {
                FreeLibrary(opengldll);
            }

            public static IntPtr GetProc(string name)
            {                
                // Try and get the proc from the opengl get proc func
                IntPtr proc = wglGetProcAddress(name);
                if (proc == IntPtr.Zero)
                {
                    // Failing that lets fallback and try and see if we can get it from the opengl32 lib (1.1 stuff)
                    proc = GetProcAddress(opengldll, name);
                }

                return proc;
            }        

        #endif

        /// <summary>
        /// A list of the GL functions that were loaded, or at least supposedly loaded, depending if GetProc returned a function actually supported.
        /// </summary>
        internal static List<string> _loadedFunctions = new List<string>();

        /// <summary>
        /// Load a OGL function based upon the delegate type passed in.
        /// </summary>
        /// <returns>callable Delegate</returns>   
        public static T LoadFunction<T>()
        {
            Type delegateType = typeof(T);
            string name = delegateType.Name;
            IntPtr proc = GetProc(name);
            if (proc == IntPtr.Zero)
            {
                return default(T);
            }

            //  Get the delegate for the function pointer.
            Delegate instance = Marshal.GetDelegateForFunctionPointer(proc, delegateType);
            if (instance == null)
            {
                throw new Exception(String.Format("Extension {0} failed to marshal correctly", delegateType.Name));
            }

            if (!_loadedFunctions.Contains(name))
            {
                _loadedFunctions.Add(name);
            }            

            return (T)(object)instance;
        }        

        /// <summary>
        /// Is a specific GL function actually loaded, i.e. available.
        /// Note: Depending what you want to achieve, you may instead want to use extension checking.
        /// </summary>
        /// <param name="name">The OpenGL name of function you want to use, not the friendly C# version.</param>
        /// <returns>true/false</returns>
        public static bool IsFunctionLoaded(string name)
        {
            return _loadedFunctions.Contains(name);
        }

        // Some basic type mapping from c# types to GL type constants
        public static Dictionary<Type, uint> TypeMapping = new Dictionary<Type, uint>() 
        {
            {typeof(uint), GL.GL_UNSIGNED_INT},
            {typeof(int), GL.GL_INT},
            {typeof(float), GL.GL_FLOAT}, 
            {typeof(double), GL.GL_DOUBLE}, 
            {typeof(sbyte), GL.GL_BYTE}, 
            {typeof(byte), GL.GL_UNSIGNED_BYTE}, 
            {typeof(short), GL.GL_SHORT},
            {typeof(ushort), GL.GL_UNSIGNED_SHORT},
        };   

        // Generic Callbacks that it's just we've hardcoded in rather than generated, as it's simpler.
        public delegate void DebugProc(uint source, uint type, uint id, uint severity, int length, string message, IntPtr userParam);
        public delegate void DebugProcArb(int id, uint category, uint severity, IntPtr length, string message, IntPtr userParam);
        public delegate void DebugProcAmd(int id, uint category, uint severity, IntPtr length, string message, IntPtr userParam);
        public delegate void DebugProcKhr(int id, uint category, uint severity, IntPtr length, string message, IntPtr userParam);

        /// <summary>
        /// An attribute that specifies version details
        /// </summary>
        public class VersionAttribute : Attribute
        {
            public string Group {get; set; }
            public string EntryPoint { get; set; }
            public bool IsExtension { get; set;}
            public string Version { get; set; }
            public string Profile { get; set; }            
            public string DeprecatedVersion { get; set; }
            public string DeprecatedProfile { get; set; }
        }     

        #region Features

        #region GL_VERSION_1_0

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCullFace(uint mode);
        internal static glCullFace _glCullFace;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCullFace", IsExtension=false)]
        public static void CullFace(uint mode)
        {
            _glCullFace(mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFrontFace(uint mode);
        internal static glFrontFace _glFrontFace;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFrontFace", IsExtension=false)]
        public static void FrontFace(uint mode)
        {
            _glFrontFace(mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glHint(uint target, uint mode);
        internal static glHint _glHint;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glHint", IsExtension=false)]
        public static void Hint(uint target, uint mode)
        {
            _glHint(target, mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glLineWidth(float width);
        internal static glLineWidth _glLineWidth;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glLineWidth", IsExtension=false)]
        public static void LineWidth(float width)
        {
            _glLineWidth(width);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPointSize(float size);
        internal static glPointSize _glPointSize;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPointSize", IsExtension=false)]
        public static void PointSize(float size)
        {
            _glPointSize(size);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPolygonMode(uint face, uint mode);
        internal static glPolygonMode _glPolygonMode;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPolygonMode", IsExtension=false)]
        public static void PolygonMode(uint face, uint mode)
        {
            _glPolygonMode(face, mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glScissor(int x, int y, int width, int height);
        internal static glScissor _glScissor;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glScissor", IsExtension=false)]
        public static void Scissor(int x, int y, int width, int height)
        {
            _glScissor(x, y, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexParameterf(uint target, uint pname, float param);
        internal static glTexParameterf _glTexParameterf;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexParameterf", IsExtension=false)]
        public static void TexParameterf(uint target, uint pname, float param)
        {
            _glTexParameterf(target, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexParameterfv(uint target, uint pname, float[] @params);
        internal static glTexParameterfv _glTexParameterfv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexParameterfv", IsExtension=false)]
        public static void TexParameterfv(uint target, uint pname, float[] @params)
        {
            _glTexParameterfv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexParameteri(uint target, uint pname, int param);
        internal static glTexParameteri _glTexParameteri;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexParameteri", IsExtension=false)]
        public static void TexParameteri(uint target, uint pname, int param)
        {
            _glTexParameteri(target, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexParameteriv(uint target, uint pname, int[] @params);
        internal static glTexParameteriv _glTexParameteriv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexParameteriv", IsExtension=false)]
        public static void TexParameteriv(uint target, uint pname, int[] @params)
        {
            _glTexParameteriv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels);
        internal static glTexImage1D _glTexImage1D;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexImage1D", IsExtension=false)]
        public static void TexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, IntPtr pixels)
        {
            _glTexImage1D(target, level, internalformat, width, border, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
        internal static glTexImage2D _glTexImage2D;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexImage2D", IsExtension=false)]
        public static void TexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels)
        {
            _glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawBuffer(uint buf);
        internal static glDrawBuffer _glDrawBuffer;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawBuffer", IsExtension=false)]
        public static void DrawBuffer(uint buf)
        {
            _glDrawBuffer(buf);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClear(uint mask);
        internal static glClear _glClear;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClear", IsExtension=false)]
        public static void Clear(uint mask)
        {
            _glClear(mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearColor(float red, float green, float blue, float alpha);
        internal static glClearColor _glClearColor;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearColor", IsExtension=false)]
        public static void ClearColor(float red, float green, float blue, float alpha)
        {
            _glClearColor(red, green, blue, alpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearStencil(int s);
        internal static glClearStencil _glClearStencil;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearStencil", IsExtension=false)]
        public static void ClearStencil(int s)
        {
            _glClearStencil(s);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearDepth(double depth);
        internal static glClearDepth _glClearDepth;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearDepth", IsExtension=false)]
        public static void ClearDepth(double depth)
        {
            _glClearDepth(depth);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilMask(uint mask);
        internal static glStencilMask _glStencilMask;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilMask", IsExtension=false)]
        public static void StencilMask(uint mask)
        {
            _glStencilMask(mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glColorMask(bool red, bool green, bool blue, bool alpha);
        internal static glColorMask _glColorMask;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glColorMask", IsExtension=false)]
        public static void ColorMask(bool red, bool green, bool blue, bool alpha)
        {
            _glColorMask(red, green, blue, alpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDepthMask(bool flag);
        internal static glDepthMask _glDepthMask;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDepthMask", IsExtension=false)]
        public static void DepthMask(bool flag)
        {
            _glDepthMask(flag);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDisable(uint cap);
        internal static glDisable _glDisable;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDisable", IsExtension=false)]
        public static void Disable(uint cap)
        {
            _glDisable(cap);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEnable(uint cap);
        internal static glEnable _glEnable;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEnable", IsExtension=false)]
        public static void Enable(uint cap)
        {
            _glEnable(cap);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFinish();
        internal static glFinish _glFinish;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFinish", IsExtension=false)]
        public static void Finish()
        {
            _glFinish();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFlush();
        internal static glFlush _glFlush;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFlush", IsExtension=false)]
        public static void Flush()
        {
            _glFlush();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendFunc(uint sfactor, uint dfactor);
        internal static glBlendFunc _glBlendFunc;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendFunc", IsExtension=false)]
        public static void BlendFunc(uint sfactor, uint dfactor)
        {
            _glBlendFunc(sfactor, dfactor);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glLogicOp(uint opcode);
        internal static glLogicOp _glLogicOp;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glLogicOp", IsExtension=false)]
        public static void LogicOp(uint opcode)
        {
            _glLogicOp(opcode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilFunc(uint func, int @ref, uint mask);
        internal static glStencilFunc _glStencilFunc;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilFunc", IsExtension=false)]
        public static void StencilFunc(uint func, int @ref, uint mask)
        {
            _glStencilFunc(func, @ref, mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilOp(uint fail, uint zfail, uint zpass);
        internal static glStencilOp _glStencilOp;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilOp", IsExtension=false)]
        public static void StencilOp(uint fail, uint zfail, uint zpass)
        {
            _glStencilOp(fail, zfail, zpass);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDepthFunc(uint func);
        internal static glDepthFunc _glDepthFunc;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDepthFunc", IsExtension=false)]
        public static void DepthFunc(uint func)
        {
            _glDepthFunc(func);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPixelStoref(uint pname, float param);
        internal static glPixelStoref _glPixelStoref;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPixelStoref", IsExtension=false)]
        public static void PixelStoref(uint pname, float param)
        {
            _glPixelStoref(pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPixelStorei(uint pname, int param);
        internal static glPixelStorei _glPixelStorei;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPixelStorei", IsExtension=false)]
        public static void PixelStorei(uint pname, int param)
        {
            _glPixelStorei(pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glReadBuffer(uint src);
        internal static glReadBuffer _glReadBuffer;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glReadBuffer", IsExtension=false)]
        public static void ReadBuffer(uint src)
        {
            _glReadBuffer(src);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pixels);
        internal static glReadPixels _glReadPixels;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glReadPixels", IsExtension=false)]
        public static void ReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pixels)
        {
            _glReadPixels(x, y, width, height, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetBooleanv(uint pname, bool[] data);
        internal static glGetBooleanv _glGetBooleanv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetBooleanv", IsExtension=false)]
        public static void GetBooleanv(uint pname, bool[] data)
        {
            _glGetBooleanv(pname, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetDoublev(uint pname, double[] data);
        internal static glGetDoublev _glGetDoublev;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetDoublev", IsExtension=false)]
        public static void GetDoublev(uint pname, double[] data)
        {
            _glGetDoublev(pname, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetError();
        internal static glGetError _glGetError;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetError", IsExtension=false)]
        public static uint GetError()
        {
            uint data = _glGetError();
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetFloatv(uint pname, float[] data);
        internal static glGetFloatv _glGetFloatv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetFloatv", IsExtension=false)]
        public static void GetFloatv(uint pname, float[] data)
        {
            _glGetFloatv(pname, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetIntegerv(uint pname, int[] data);
        internal static glGetIntegerv _glGetIntegerv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetIntegerv", IsExtension=false)]
        public static void GetIntegerv(uint pname, int[] data)
        {
            _glGetIntegerv(pname, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glGetString(uint name);
        internal static glGetString _glGetString;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetString", IsExtension=false)]
        public static string GetString(uint name)
        {
            IntPtr data = _glGetString(name);
            return Marshal.PtrToStringAnsi(data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTexImage(uint target, int level, uint format, uint type, IntPtr pixels);
        internal static glGetTexImage _glGetTexImage;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTexImage", IsExtension=false)]
        public static void GetTexImage(uint target, int level, uint format, uint type, IntPtr pixels)
        {
            _glGetTexImage(target, level, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTexParameterfv(uint target, uint pname, float[] @params);
        internal static glGetTexParameterfv _glGetTexParameterfv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTexParameterfv", IsExtension=false)]
        public static void GetTexParameterfv(uint target, uint pname, float[] @params)
        {
            _glGetTexParameterfv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTexParameteriv(uint target, uint pname, int[] @params);
        internal static glGetTexParameteriv _glGetTexParameteriv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTexParameteriv", IsExtension=false)]
        public static void GetTexParameteriv(uint target, uint pname, int[] @params)
        {
            _glGetTexParameteriv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTexLevelParameterfv(uint target, int level, uint pname, float[] @params);
        internal static glGetTexLevelParameterfv _glGetTexLevelParameterfv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTexLevelParameterfv", IsExtension=false)]
        public static void GetTexLevelParameterfv(uint target, int level, uint pname, float[] @params)
        {
            _glGetTexLevelParameterfv(target, level, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTexLevelParameteriv(uint target, int level, uint pname, int[] @params);
        internal static glGetTexLevelParameteriv _glGetTexLevelParameteriv;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTexLevelParameteriv", IsExtension=false)]
        public static void GetTexLevelParameteriv(uint target, int level, uint pname, int[] @params)
        {
            _glGetTexLevelParameteriv(target, level, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsEnabled(uint cap);
        internal static glIsEnabled _glIsEnabled;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsEnabled", IsExtension=false)]
        public static bool IsEnabled(uint cap)
        {
            bool data = _glIsEnabled(cap);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDepthRange(double near, double far);
        internal static glDepthRange _glDepthRange;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDepthRange", IsExtension=false)]
        public static void DepthRange(double near, double far)
        {
            _glDepthRange(near, far);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glViewport(int x, int y, int width, int height);
        internal static glViewport _glViewport;

        [Version(Group="GL_VERSION_1_0", Version = "1.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glViewport", IsExtension=false)]
        public static void Viewport(int x, int y, int width, int height)
        {
            _glViewport(x, y, width, height);
        }
        

        #endregion

        #region GL_VERSION_1_1
        public const uint GL_DEPTH_BUFFER_BIT = (uint)0x00000100;
        public const uint GL_STENCIL_BUFFER_BIT = (uint)0x00000400;
        public const uint GL_COLOR_BUFFER_BIT = (uint)0x00004000;
        public const uint GL_FALSE = (uint)0;
        public const uint GL_TRUE = (uint)1;
        public const uint GL_POINTS = (uint)0x0000;
        public const uint GL_LINES = (uint)0x0001;
        public const uint GL_LINE_LOOP = (uint)0x0002;
        public const uint GL_LINE_STRIP = (uint)0x0003;
        public const uint GL_TRIANGLES = (uint)0x0004;
        public const uint GL_TRIANGLE_STRIP = (uint)0x0005;
        public const uint GL_TRIANGLE_FAN = (uint)0x0006;
        public const uint GL_NEVER = (uint)0x0200;
        public const uint GL_LESS = (uint)0x0201;
        public const uint GL_EQUAL = (uint)0x0202;
        public const uint GL_LEQUAL = (uint)0x0203;
        public const uint GL_GREATER = (uint)0x0204;
        public const uint GL_NOTEQUAL = (uint)0x0205;
        public const uint GL_GEQUAL = (uint)0x0206;
        public const uint GL_ALWAYS = (uint)0x0207;
        public const uint GL_ZERO = (uint)0;
        public const uint GL_ONE = (uint)1;
        public const uint GL_SRC_COLOR = (uint)0x0300;
        public const uint GL_ONE_MINUS_SRC_COLOR = (uint)0x0301;
        public const uint GL_SRC_ALPHA = (uint)0x0302;
        public const uint GL_ONE_MINUS_SRC_ALPHA = (uint)0x0303;
        public const uint GL_DST_ALPHA = (uint)0x0304;
        public const uint GL_ONE_MINUS_DST_ALPHA = (uint)0x0305;
        public const uint GL_DST_COLOR = (uint)0x0306;
        public const uint GL_ONE_MINUS_DST_COLOR = (uint)0x0307;
        public const uint GL_SRC_ALPHA_SATURATE = (uint)0x0308;
        public const uint GL_FRONT_LEFT = (uint)0x0400;
        public const uint GL_FRONT_RIGHT = (uint)0x0401;
        public const uint GL_BACK_LEFT = (uint)0x0402;
        public const uint GL_BACK_RIGHT = (uint)0x0403;
        public const uint GL_FRONT = (uint)0x0404;
        public const uint GL_LEFT = (uint)0x0406;
        public const uint GL_RIGHT = (uint)0x0407;
        public const uint GL_FRONT_AND_BACK = (uint)0x0408;
        public const uint GL_INVALID_ENUM = (uint)0x0500;
        public const uint GL_INVALID_VALUE = (uint)0x0501;
        public const uint GL_INVALID_OPERATION = (uint)0x0502;
        public const uint GL_OUT_OF_MEMORY = (uint)0x0505;
        public const uint GL_CW = (uint)0x0900;
        public const uint GL_CCW = (uint)0x0901;
        public const uint GL_POINT_SIZE = (uint)0x0B11;
        public const uint GL_POINT_SIZE_RANGE = (uint)0x0B12;
        public const uint GL_POINT_SIZE_GRANULARITY = (uint)0x0B13;
        public const uint GL_LINE_SMOOTH = (uint)0x0B20;
        public const uint GL_LINE_WIDTH = (uint)0x0B21;
        public const uint GL_LINE_WIDTH_RANGE = (uint)0x0B22;
        public const uint GL_LINE_WIDTH_GRANULARITY = (uint)0x0B23;
        public const uint GL_POLYGON_MODE = (uint)0x0B40;
        public const uint GL_POLYGON_SMOOTH = (uint)0x0B41;
        public const uint GL_CULL_FACE = (uint)0x0B44;
        public const uint GL_CULL_FACE_MODE = (uint)0x0B45;
        public const uint GL_FRONT_FACE = (uint)0x0B46;
        public const uint GL_DEPTH_RANGE = (uint)0x0B70;
        public const uint GL_DEPTH_TEST = (uint)0x0B71;
        public const uint GL_DEPTH_WRITEMASK = (uint)0x0B72;
        public const uint GL_DEPTH_CLEAR_VALUE = (uint)0x0B73;
        public const uint GL_DEPTH_FUNC = (uint)0x0B74;
        public const uint GL_STENCIL_TEST = (uint)0x0B90;
        public const uint GL_STENCIL_CLEAR_VALUE = (uint)0x0B91;
        public const uint GL_STENCIL_FUNC = (uint)0x0B92;
        public const uint GL_STENCIL_VALUE_MASK = (uint)0x0B93;
        public const uint GL_STENCIL_FAIL = (uint)0x0B94;
        public const uint GL_STENCIL_PASS_DEPTH_FAIL = (uint)0x0B95;
        public const uint GL_STENCIL_PASS_DEPTH_PASS = (uint)0x0B96;
        public const uint GL_STENCIL_REF = (uint)0x0B97;
        public const uint GL_STENCIL_WRITEMASK = (uint)0x0B98;
        public const uint GL_VIEWPORT = (uint)0x0BA2;
        public const uint GL_DITHER = (uint)0x0BD0;
        public const uint GL_BLEND_DST = (uint)0x0BE0;
        public const uint GL_BLEND_SRC = (uint)0x0BE1;
        public const uint GL_BLEND = (uint)0x0BE2;
        public const uint GL_LOGIC_OP_MODE = (uint)0x0BF0;
        public const uint GL_COLOR_LOGIC_OP = (uint)0x0BF2;
        public const uint GL_DRAW_BUFFER = (uint)0x0C01;
        public const uint GL_READ_BUFFER = (uint)0x0C02;
        public const uint GL_SCISSOR_BOX = (uint)0x0C10;
        public const uint GL_SCISSOR_TEST = (uint)0x0C11;
        public const uint GL_COLOR_CLEAR_VALUE = (uint)0x0C22;
        public const uint GL_COLOR_WRITEMASK = (uint)0x0C23;
        public const uint GL_DOUBLEBUFFER = (uint)0x0C32;
        public const uint GL_STEREO = (uint)0x0C33;
        public const uint GL_LINE_SMOOTH_HINT = (uint)0x0C52;
        public const uint GL_POLYGON_SMOOTH_HINT = (uint)0x0C53;
        public const uint GL_UNPACK_SWAP_BYTES = (uint)0x0CF0;
        public const uint GL_UNPACK_LSB_FIRST = (uint)0x0CF1;
        public const uint GL_UNPACK_ROW_LENGTH = (uint)0x0CF2;
        public const uint GL_UNPACK_SKIP_ROWS = (uint)0x0CF3;
        public const uint GL_UNPACK_SKIP_PIXELS = (uint)0x0CF4;
        public const uint GL_UNPACK_ALIGNMENT = (uint)0x0CF5;
        public const uint GL_PACK_SWAP_BYTES = (uint)0x0D00;
        public const uint GL_PACK_LSB_FIRST = (uint)0x0D01;
        public const uint GL_PACK_ROW_LENGTH = (uint)0x0D02;
        public const uint GL_PACK_SKIP_ROWS = (uint)0x0D03;
        public const uint GL_PACK_SKIP_PIXELS = (uint)0x0D04;
        public const uint GL_PACK_ALIGNMENT = (uint)0x0D05;
        public const uint GL_MAX_TEXTURE_SIZE = (uint)0x0D33;
        public const uint GL_MAX_VIEWPORT_DIMS = (uint)0x0D3A;
        public const uint GL_SUBPIXEL_BITS = (uint)0x0D50;
        public const uint GL_TEXTURE_1D = (uint)0x0DE0;
        public const uint GL_TEXTURE_2D = (uint)0x0DE1;
        public const uint GL_POLYGON_OFFSET_UNITS = (uint)0x2A00;
        public const uint GL_POLYGON_OFFSET_POINT = (uint)0x2A01;
        public const uint GL_POLYGON_OFFSET_LINE = (uint)0x2A02;
        public const uint GL_POLYGON_OFFSET_FILL = (uint)0x8037;
        public const uint GL_POLYGON_OFFSET_FACTOR = (uint)0x8038;
        public const uint GL_TEXTURE_WIDTH = (uint)0x1000;
        public const uint GL_TEXTURE_HEIGHT = (uint)0x1001;
        public const uint GL_TEXTURE_INTERNAL_FORMAT = (uint)0x1003;
        public const uint GL_TEXTURE_BORDER_COLOR = (uint)0x1004;
        public const uint GL_TEXTURE_RED_SIZE = (uint)0x805C;
        public const uint GL_TEXTURE_GREEN_SIZE = (uint)0x805D;
        public const uint GL_TEXTURE_BLUE_SIZE = (uint)0x805E;
        public const uint GL_TEXTURE_ALPHA_SIZE = (uint)0x805F;
        public const uint GL_DONT_CARE = (uint)0x1100;
        public const uint GL_FASTEST = (uint)0x1101;
        public const uint GL_NICEST = (uint)0x1102;
        public const uint GL_BYTE = (uint)0x1400;
        public const uint GL_UNSIGNED_BYTE = (uint)0x1401;
        public const uint GL_SHORT = (uint)0x1402;
        public const uint GL_UNSIGNED_SHORT = (uint)0x1403;
        public const uint GL_INT = (uint)0x1404;
        public const uint GL_UNSIGNED_INT = (uint)0x1405;
        public const uint GL_FLOAT = (uint)0x1406;
        public const uint GL_DOUBLE = (uint)0x140A;
        public const uint GL_CLEAR = (uint)0x1500;
        public const uint GL_AND = (uint)0x1501;
        public const uint GL_AND_REVERSE = (uint)0x1502;
        public const uint GL_COPY = (uint)0x1503;
        public const uint GL_AND_INVERTED = (uint)0x1504;
        public const uint GL_NOOP = (uint)0x1505;
        public const uint GL_XOR = (uint)0x1506;
        public const uint GL_OR = (uint)0x1507;
        public const uint GL_NOR = (uint)0x1508;
        public const uint GL_EQUIV = (uint)0x1509;
        public const uint GL_INVERT = (uint)0x150A;
        public const uint GL_OR_REVERSE = (uint)0x150B;
        public const uint GL_COPY_INVERTED = (uint)0x150C;
        public const uint GL_OR_INVERTED = (uint)0x150D;
        public const uint GL_NAND = (uint)0x150E;
        public const uint GL_SET = (uint)0x150F;
        public const uint GL_TEXTURE = (uint)0x1702;
        public const uint GL_COLOR = (uint)0x1800;
        public const uint GL_DEPTH = (uint)0x1801;
        public const uint GL_STENCIL = (uint)0x1802;
        public const uint GL_DEPTH_COMPONENT = (uint)0x1902;
        public const uint GL_RED = (uint)0x1903;
        public const uint GL_GREEN = (uint)0x1904;
        public const uint GL_BLUE = (uint)0x1905;
        public const uint GL_ALPHA = (uint)0x1906;
        public const uint GL_RGB = (uint)0x1907;
        public const uint GL_RGBA = (uint)0x1908;
        public const uint GL_POINT = (uint)0x1B00;
        public const uint GL_LINE = (uint)0x1B01;
        public const uint GL_FILL = (uint)0x1B02;
        public const uint GL_KEEP = (uint)0x1E00;
        public const uint GL_REPLACE = (uint)0x1E01;
        public const uint GL_INCR = (uint)0x1E02;
        public const uint GL_DECR = (uint)0x1E03;
        public const uint GL_VENDOR = (uint)0x1F00;
        public const uint GL_RENDERER = (uint)0x1F01;
        public const uint GL_VERSION = (uint)0x1F02;
        public const uint GL_EXTENSIONS = (uint)0x1F03;
        public const uint GL_NEAREST = (uint)0x2600;
        public const uint GL_LINEAR = (uint)0x2601;
        public const uint GL_NEAREST_MIPMAP_NEAREST = (uint)0x2700;
        public const uint GL_LINEAR_MIPMAP_NEAREST = (uint)0x2701;
        public const uint GL_NEAREST_MIPMAP_LINEAR = (uint)0x2702;
        public const uint GL_LINEAR_MIPMAP_LINEAR = (uint)0x2703;
        public const uint GL_TEXTURE_MAG_FILTER = (uint)0x2800;
        public const uint GL_TEXTURE_MIN_FILTER = (uint)0x2801;
        public const uint GL_TEXTURE_WRAP_S = (uint)0x2802;
        public const uint GL_TEXTURE_WRAP_T = (uint)0x2803;
        public const uint GL_PROXY_TEXTURE_1D = (uint)0x8063;
        public const uint GL_PROXY_TEXTURE_2D = (uint)0x8064;
        public const uint GL_REPEAT = (uint)0x2901;
        public const uint GL_R3_G3_B2 = (uint)0x2A10;
        public const uint GL_RGB4 = (uint)0x804F;
        public const uint GL_RGB5 = (uint)0x8050;
        public const uint GL_RGB8 = (uint)0x8051;
        public const uint GL_RGB10 = (uint)0x8052;
        public const uint GL_RGB12 = (uint)0x8053;
        public const uint GL_RGB16 = (uint)0x8054;
        public const uint GL_RGBA2 = (uint)0x8055;
        public const uint GL_RGBA4 = (uint)0x8056;
        public const uint GL_RGB5_A1 = (uint)0x8057;
        public const uint GL_RGBA8 = (uint)0x8058;
        public const uint GL_RGB10_A2 = (uint)0x8059;
        public const uint GL_RGBA12 = (uint)0x805A;
        public const uint GL_RGBA16 = (uint)0x805B;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawArrays(uint mode, int first, int count);
        internal static glDrawArrays _glDrawArrays;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawArrays", IsExtension=false)]
        public static void DrawArrays(uint mode, int first, int count)
        {
            _glDrawArrays(mode, first, count);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElements(uint mode, int count, uint type, IntPtr indices);
        internal static glDrawElements _glDrawElements;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElements", IsExtension=false)]
        public static void DrawElements(uint mode, int count, uint type, IntPtr indices)
        {
            _glDrawElements(mode, count, type, indices);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPolygonOffset(float factor, float units);
        internal static glPolygonOffset _glPolygonOffset;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPolygonOffset", IsExtension=false)]
        public static void PolygonOffset(float factor, float units)
        {
            _glPolygonOffset(factor, units);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTexImage1D(uint target, int level, uint internalformat, int x, int y, int width, int border);
        internal static glCopyTexImage1D _glCopyTexImage1D;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTexImage1D", IsExtension=false)]
        public static void CopyTexImage1D(uint target, int level, uint internalformat, int x, int y, int width, int border)
        {
            _glCopyTexImage1D(target, level, internalformat, x, y, width, border);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border);
        internal static glCopyTexImage2D _glCopyTexImage2D;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTexImage2D", IsExtension=false)]
        public static void CopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border)
        {
            _glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width);
        internal static glCopyTexSubImage1D _glCopyTexSubImage1D;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTexSubImage1D", IsExtension=false)]
        public static void CopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width)
        {
            _glCopyTexSubImage1D(target, level, xoffset, x, y, width);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
        internal static glCopyTexSubImage2D _glCopyTexSubImage2D;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTexSubImage2D", IsExtension=false)]
        public static void CopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height)
        {
            _glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
        internal static glTexSubImage1D _glTexSubImage1D;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexSubImage1D", IsExtension=false)]
        public static void TexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, IntPtr pixels)
        {
            _glTexSubImage1D(target, level, xoffset, width, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
        internal static glTexSubImage2D _glTexSubImage2D;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexSubImage2D", IsExtension=false)]
        public static void TexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels)
        {
            _glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindTexture(uint target, uint texture);
        internal static glBindTexture _glBindTexture;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindTexture", IsExtension=false)]
        public static void BindTexture(uint target, uint texture)
        {
            _glBindTexture(target, texture);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteTextures(int n, uint[] textures);
        internal static glDeleteTextures _glDeleteTextures;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteTextures", IsExtension=false)]
        public static void DeleteTextures(int n, uint[] textures)
        {
            _glDeleteTextures(n, textures);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenTextures(int n, uint[] textures);
        internal static glGenTextures _glGenTextures;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenTextures", IsExtension=false)]
        public static void GenTextures(int n, uint[] textures)
        {
            _glGenTextures(n, textures);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsTexture(uint texture);
        internal static glIsTexture _glIsTexture;

        [Version(Group="GL_VERSION_1_1", Version = "1.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsTexture", IsExtension=false)]
        public static bool IsTexture(uint texture)
        {
            bool data = _glIsTexture(texture);
            return data;
        }
        

        #endregion

        #region GL_VERSION_1_2
        public const uint GL_UNSIGNED_BYTE_3_3_2 = (uint)0x8032;
        public const uint GL_UNSIGNED_SHORT_4_4_4_4 = (uint)0x8033;
        public const uint GL_UNSIGNED_SHORT_5_5_5_1 = (uint)0x8034;
        public const uint GL_UNSIGNED_INT_8_8_8_8 = (uint)0x8035;
        public const uint GL_UNSIGNED_INT_10_10_10_2 = (uint)0x8036;
        public const uint GL_PACK_SKIP_IMAGES = (uint)0x806B;
        public const uint GL_PACK_IMAGE_HEIGHT = (uint)0x806C;
        public const uint GL_UNPACK_SKIP_IMAGES = (uint)0x806D;
        public const uint GL_UNPACK_IMAGE_HEIGHT = (uint)0x806E;
        public const uint GL_TEXTURE_3D = (uint)0x806F;
        public const uint GL_PROXY_TEXTURE_3D = (uint)0x8070;
        public const uint GL_TEXTURE_DEPTH = (uint)0x8071;
        public const uint GL_TEXTURE_WRAP_R = (uint)0x8072;
        public const uint GL_MAX_3D_TEXTURE_SIZE = (uint)0x8073;
        public const uint GL_UNSIGNED_BYTE_2_3_3_REV = (uint)0x8362;
        public const uint GL_UNSIGNED_SHORT_5_6_5 = (uint)0x8363;
        public const uint GL_UNSIGNED_SHORT_5_6_5_REV = (uint)0x8364;
        public const uint GL_UNSIGNED_SHORT_4_4_4_4_REV = (uint)0x8365;
        public const uint GL_UNSIGNED_SHORT_1_5_5_5_REV = (uint)0x8366;
        public const uint GL_UNSIGNED_INT_8_8_8_8_REV = (uint)0x8367;
        public const uint GL_UNSIGNED_INT_2_10_10_10_REV = (uint)0x8368;
        public const uint GL_BGR = (uint)0x80E0;
        public const uint GL_BGRA = (uint)0x80E1;
        public const uint GL_MAX_ELEMENTS_VERTICES = (uint)0x80E8;
        public const uint GL_MAX_ELEMENTS_INDICES = (uint)0x80E9;
        public const uint GL_CLAMP_TO_EDGE = (uint)0x812F;
        public const uint GL_TEXTURE_MIN_LOD = (uint)0x813A;
        public const uint GL_TEXTURE_MAX_LOD = (uint)0x813B;
        public const uint GL_TEXTURE_BASE_LEVEL = (uint)0x813C;
        public const uint GL_TEXTURE_MAX_LEVEL = (uint)0x813D;
        public const uint GL_SMOOTH_POINT_SIZE_RANGE = (uint)0x0B12;
        public const uint GL_SMOOTH_POINT_SIZE_GRANULARITY = (uint)0x0B13;
        public const uint GL_SMOOTH_LINE_WIDTH_RANGE = (uint)0x0B22;
        public const uint GL_SMOOTH_LINE_WIDTH_GRANULARITY = (uint)0x0B23;
        public const uint GL_ALIASED_LINE_WIDTH_RANGE = (uint)0x846E;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawRangeElements(uint mode, uint start, uint end, int count, uint type, IntPtr indices);
        internal static glDrawRangeElements _glDrawRangeElements;

        [Version(Group="GL_VERSION_1_2", Version = "1.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawRangeElements", IsExtension=false)]
        public static void DrawRangeElements(uint mode, uint start, uint end, int count, uint type, IntPtr indices)
        {
            _glDrawRangeElements(mode, start, end, count, type, indices);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels);
        internal static glTexImage3D _glTexImage3D;

        [Version(Group="GL_VERSION_1_2", Version = "1.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexImage3D", IsExtension=false)]
        public static void TexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels)
        {
            _glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
        internal static glTexSubImage3D _glTexSubImage3D;

        [Version(Group="GL_VERSION_1_2", Version = "1.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexSubImage3D", IsExtension=false)]
        public static void TexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels)
        {
            _glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
        internal static glCopyTexSubImage3D _glCopyTexSubImage3D;

        [Version(Group="GL_VERSION_1_2", Version = "1.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTexSubImage3D", IsExtension=false)]
        public static void CopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height)
        {
            _glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height);
        }
        

        #endregion

        #region GL_VERSION_1_3
        public const uint GL_TEXTURE0 = (uint)0x84C0;
        public const uint GL_TEXTURE1 = (uint)0x84C1;
        public const uint GL_TEXTURE2 = (uint)0x84C2;
        public const uint GL_TEXTURE3 = (uint)0x84C3;
        public const uint GL_TEXTURE4 = (uint)0x84C4;
        public const uint GL_TEXTURE5 = (uint)0x84C5;
        public const uint GL_TEXTURE6 = (uint)0x84C6;
        public const uint GL_TEXTURE7 = (uint)0x84C7;
        public const uint GL_TEXTURE8 = (uint)0x84C8;
        public const uint GL_TEXTURE9 = (uint)0x84C9;
        public const uint GL_TEXTURE10 = (uint)0x84CA;
        public const uint GL_TEXTURE11 = (uint)0x84CB;
        public const uint GL_TEXTURE12 = (uint)0x84CC;
        public const uint GL_TEXTURE13 = (uint)0x84CD;
        public const uint GL_TEXTURE14 = (uint)0x84CE;
        public const uint GL_TEXTURE15 = (uint)0x84CF;
        public const uint GL_TEXTURE16 = (uint)0x84D0;
        public const uint GL_TEXTURE17 = (uint)0x84D1;
        public const uint GL_TEXTURE18 = (uint)0x84D2;
        public const uint GL_TEXTURE19 = (uint)0x84D3;
        public const uint GL_TEXTURE20 = (uint)0x84D4;
        public const uint GL_TEXTURE21 = (uint)0x84D5;
        public const uint GL_TEXTURE22 = (uint)0x84D6;
        public const uint GL_TEXTURE23 = (uint)0x84D7;
        public const uint GL_TEXTURE24 = (uint)0x84D8;
        public const uint GL_TEXTURE25 = (uint)0x84D9;
        public const uint GL_TEXTURE26 = (uint)0x84DA;
        public const uint GL_TEXTURE27 = (uint)0x84DB;
        public const uint GL_TEXTURE28 = (uint)0x84DC;
        public const uint GL_TEXTURE29 = (uint)0x84DD;
        public const uint GL_TEXTURE30 = (uint)0x84DE;
        public const uint GL_TEXTURE31 = (uint)0x84DF;
        public const uint GL_ACTIVE_TEXTURE = (uint)0x84E0;
        public const uint GL_MULTISAMPLE = (uint)0x809D;
        public const uint GL_SAMPLE_ALPHA_TO_COVERAGE = (uint)0x809E;
        public const uint GL_SAMPLE_ALPHA_TO_ONE = (uint)0x809F;
        public const uint GL_SAMPLE_COVERAGE = (uint)0x80A0;
        public const uint GL_SAMPLE_BUFFERS = (uint)0x80A8;
        public const uint GL_SAMPLES = (uint)0x80A9;
        public const uint GL_SAMPLE_COVERAGE_VALUE = (uint)0x80AA;
        public const uint GL_SAMPLE_COVERAGE_INVERT = (uint)0x80AB;
        public const uint GL_TEXTURE_CUBE_MAP = (uint)0x8513;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_X = (uint)0x8515;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_X = (uint)0x8516;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Y = (uint)0x8517;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = (uint)0x8518;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Z = (uint)0x8519;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = (uint)0x851A;
        public const uint GL_PROXY_TEXTURE_CUBE_MAP = (uint)0x851B;
        public const uint GL_MAX_CUBE_MAP_TEXTURE_SIZE = (uint)0x851C;
        public const uint GL_COMPRESSED_RGB = (uint)0x84ED;
        public const uint GL_COMPRESSED_RGBA = (uint)0x84EE;
        public const uint GL_TEXTURE_COMPRESSION_HINT = (uint)0x84EF;
        public const uint GL_TEXTURE_COMPRESSED_IMAGE_SIZE = (uint)0x86A0;
        public const uint GL_TEXTURE_COMPRESSED = (uint)0x86A1;
        public const uint GL_NUM_COMPRESSED_TEXTURE_FORMATS = (uint)0x86A2;
        public const uint GL_COMPRESSED_TEXTURE_FORMATS = (uint)0x86A3;
        public const uint GL_CLAMP_TO_BORDER = (uint)0x812D;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glActiveTexture(uint texture);
        internal static glActiveTexture _glActiveTexture;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glActiveTexture", IsExtension=false)]
        public static void ActiveTexture(uint texture)
        {
            _glActiveTexture(texture);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSampleCoverage(float value, bool invert);
        internal static glSampleCoverage _glSampleCoverage;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSampleCoverage", IsExtension=false)]
        public static void SampleCoverage(float value, bool invert)
        {
            _glSampleCoverage(value, invert);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data);
        internal static glCompressedTexImage3D _glCompressedTexImage3D;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTexImage3D", IsExtension=false)]
        public static void CompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data)
        {
            _glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data);
        internal static glCompressedTexImage2D _glCompressedTexImage2D;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTexImage2D", IsExtension=false)]
        public static void CompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data)
        {
            _glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTexImage1D(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data);
        internal static glCompressedTexImage1D _glCompressedTexImage1D;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTexImage1D", IsExtension=false)]
        public static void CompressedTexImage1D(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data)
        {
            _glCompressedTexImage1D(target, level, internalformat, width, border, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
        internal static glCompressedTexSubImage3D _glCompressedTexSubImage3D;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTexSubImage3D", IsExtension=false)]
        public static void CompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data)
        {
            _glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
        internal static glCompressedTexSubImage2D _glCompressedTexSubImage2D;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTexSubImage2D", IsExtension=false)]
        public static void CompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data)
        {
            _glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTexSubImage1D(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
        internal static glCompressedTexSubImage1D _glCompressedTexSubImage1D;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTexSubImage1D", IsExtension=false)]
        public static void CompressedTexSubImage1D(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data)
        {
            _glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetCompressedTexImage(uint target, int level, IntPtr img);
        internal static glGetCompressedTexImage _glGetCompressedTexImage;

        [Version(Group="GL_VERSION_1_3", Version = "1.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetCompressedTexImage", IsExtension=false)]
        public static void GetCompressedTexImage(uint target, int level, IntPtr img)
        {
            _glGetCompressedTexImage(target, level, img);
        }
        

        #endregion

        #region GL_VERSION_1_4
        public const uint GL_BLEND_DST_RGB = (uint)0x80C8;
        public const uint GL_BLEND_SRC_RGB = (uint)0x80C9;
        public const uint GL_BLEND_DST_ALPHA = (uint)0x80CA;
        public const uint GL_BLEND_SRC_ALPHA = (uint)0x80CB;
        public const uint GL_POINT_FADE_THRESHOLD_SIZE = (uint)0x8128;
        public const uint GL_DEPTH_COMPONENT16 = (uint)0x81A5;
        public const uint GL_DEPTH_COMPONENT24 = (uint)0x81A6;
        public const uint GL_DEPTH_COMPONENT32 = (uint)0x81A7;
        public const uint GL_MIRRORED_REPEAT = (uint)0x8370;
        public const uint GL_MAX_TEXTURE_LOD_BIAS = (uint)0x84FD;
        public const uint GL_TEXTURE_LOD_BIAS = (uint)0x8501;
        public const uint GL_INCR_WRAP = (uint)0x8507;
        public const uint GL_DECR_WRAP = (uint)0x8508;
        public const uint GL_TEXTURE_DEPTH_SIZE = (uint)0x884A;
        public const uint GL_TEXTURE_COMPARE_MODE = (uint)0x884C;
        public const uint GL_TEXTURE_COMPARE_FUNC = (uint)0x884D;
        public const uint GL_FUNC_ADD = (uint)0x8006;
        public const uint GL_FUNC_SUBTRACT = (uint)0x800A;
        public const uint GL_FUNC_REVERSE_SUBTRACT = (uint)0x800B;
        public const uint GL_MIN = (uint)0x8007;
        public const uint GL_MAX = (uint)0x8008;
        public const uint GL_CONSTANT_COLOR = (uint)0x8001;
        public const uint GL_ONE_MINUS_CONSTANT_COLOR = (uint)0x8002;
        public const uint GL_CONSTANT_ALPHA = (uint)0x8003;
        public const uint GL_ONE_MINUS_CONSTANT_ALPHA = (uint)0x8004;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
        internal static glBlendFuncSeparate _glBlendFuncSeparate;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendFuncSeparate", IsExtension=false)]
        public static void BlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha)
        {
            _glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMultiDrawArrays(uint mode, int[] first, int[] count, int drawcount);
        internal static glMultiDrawArrays _glMultiDrawArrays;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMultiDrawArrays", IsExtension=false)]
        public static void MultiDrawArrays(uint mode, int[] first, int[] count, int drawcount)
        {
            _glMultiDrawArrays(mode, first, count, drawcount);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMultiDrawElements(uint mode, int[] count, uint type, IntPtr indices, int drawcount);
        internal static glMultiDrawElements _glMultiDrawElements;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMultiDrawElements", IsExtension=false)]
        public static void MultiDrawElements(uint mode, int[] count, uint type, IntPtr indices, int drawcount)
        {
            _glMultiDrawElements(mode, count, type, indices, drawcount);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPointParameterf(uint pname, float param);
        internal static glPointParameterf _glPointParameterf;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPointParameterf", IsExtension=false)]
        public static void PointParameterf(uint pname, float param)
        {
            _glPointParameterf(pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPointParameterfv(uint pname, float[] @params);
        internal static glPointParameterfv _glPointParameterfv;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPointParameterfv", IsExtension=false)]
        public static void PointParameterfv(uint pname, float[] @params)
        {
            _glPointParameterfv(pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPointParameteri(uint pname, int param);
        internal static glPointParameteri _glPointParameteri;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPointParameteri", IsExtension=false)]
        public static void PointParameteri(uint pname, int param)
        {
            _glPointParameteri(pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPointParameteriv(uint pname, int[] @params);
        internal static glPointParameteriv _glPointParameteriv;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPointParameteriv", IsExtension=false)]
        public static void PointParameteriv(uint pname, int[] @params)
        {
            _glPointParameteriv(pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendColor(float red, float green, float blue, float alpha);
        internal static glBlendColor _glBlendColor;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendColor", IsExtension=false)]
        public static void BlendColor(float red, float green, float blue, float alpha)
        {
            _glBlendColor(red, green, blue, alpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendEquation(uint mode);
        internal static glBlendEquation _glBlendEquation;

        [Version(Group="GL_VERSION_1_4", Version = "1.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendEquation", IsExtension=false)]
        public static void BlendEquation(uint mode)
        {
            _glBlendEquation(mode);
        }
        

        #endregion

        #region GL_VERSION_1_5
        public const uint GL_BUFFER_SIZE = (uint)0x8764;
        public const uint GL_BUFFER_USAGE = (uint)0x8765;
        public const uint GL_QUERY_COUNTER_BITS = (uint)0x8864;
        public const uint GL_CURRENT_QUERY = (uint)0x8865;
        public const uint GL_QUERY_RESULT = (uint)0x8866;
        public const uint GL_QUERY_RESULT_AVAILABLE = (uint)0x8867;
        public const uint GL_ARRAY_BUFFER = (uint)0x8892;
        public const uint GL_ELEMENT_ARRAY_BUFFER = (uint)0x8893;
        public const uint GL_ARRAY_BUFFER_BINDING = (uint)0x8894;
        public const uint GL_ELEMENT_ARRAY_BUFFER_BINDING = (uint)0x8895;
        public const uint GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = (uint)0x889F;
        public const uint GL_READ_ONLY = (uint)0x88B8;
        public const uint GL_WRITE_ONLY = (uint)0x88B9;
        public const uint GL_READ_WRITE = (uint)0x88BA;
        public const uint GL_BUFFER_ACCESS = (uint)0x88BB;
        public const uint GL_BUFFER_MAPPED = (uint)0x88BC;
        public const uint GL_BUFFER_MAP_POINTER = (uint)0x88BD;
        public const uint GL_STREAM_DRAW = (uint)0x88E0;
        public const uint GL_STREAM_READ = (uint)0x88E1;
        public const uint GL_STREAM_COPY = (uint)0x88E2;
        public const uint GL_STATIC_DRAW = (uint)0x88E4;
        public const uint GL_STATIC_READ = (uint)0x88E5;
        public const uint GL_STATIC_COPY = (uint)0x88E6;
        public const uint GL_DYNAMIC_DRAW = (uint)0x88E8;
        public const uint GL_DYNAMIC_READ = (uint)0x88E9;
        public const uint GL_DYNAMIC_COPY = (uint)0x88EA;
        public const uint GL_SAMPLES_PASSED = (uint)0x8914;
        public const uint GL_SRC1_ALPHA = (uint)0x8589;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenQueries(int n, uint[] ids);
        internal static glGenQueries _glGenQueries;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenQueries", IsExtension=false)]
        public static void GenQueries(int n, uint[] ids)
        {
            _glGenQueries(n, ids);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteQueries(int n, uint[] ids);
        internal static glDeleteQueries _glDeleteQueries;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteQueries", IsExtension=false)]
        public static void DeleteQueries(int n, uint[] ids)
        {
            _glDeleteQueries(n, ids);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsQuery(uint id);
        internal static glIsQuery _glIsQuery;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsQuery", IsExtension=false)]
        public static bool IsQuery(uint id)
        {
            bool data = _glIsQuery(id);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBeginQuery(uint target, uint id);
        internal static glBeginQuery _glBeginQuery;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBeginQuery", IsExtension=false)]
        public static void BeginQuery(uint target, uint id)
        {
            _glBeginQuery(target, id);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEndQuery(uint target);
        internal static glEndQuery _glEndQuery;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEndQuery", IsExtension=false)]
        public static void EndQuery(uint target)
        {
            _glEndQuery(target);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryiv(uint target, uint pname, int[] @params);
        internal static glGetQueryiv _glGetQueryiv;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryiv", IsExtension=false)]
        public static void GetQueryiv(uint target, uint pname, int[] @params)
        {
            _glGetQueryiv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryObjectiv(uint id, uint pname, int[] @params);
        internal static glGetQueryObjectiv _glGetQueryObjectiv;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryObjectiv", IsExtension=false)]
        public static void GetQueryObjectiv(uint id, uint pname, int[] @params)
        {
            _glGetQueryObjectiv(id, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryObjectuiv(uint id, uint pname, uint[] @params);
        internal static glGetQueryObjectuiv _glGetQueryObjectuiv;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryObjectuiv", IsExtension=false)]
        public static void GetQueryObjectuiv(uint id, uint pname, uint[] @params)
        {
            _glGetQueryObjectuiv(id, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindBuffer(uint target, uint buffer);
        internal static glBindBuffer _glBindBuffer;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindBuffer", IsExtension=false)]
        public static void BindBuffer(uint target, uint buffer)
        {
            _glBindBuffer(target, buffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteBuffers(int n, uint[] buffers);
        internal static glDeleteBuffers _glDeleteBuffers;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteBuffers", IsExtension=false)]
        public static void DeleteBuffers(int n, uint[] buffers)
        {
            _glDeleteBuffers(n, buffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenBuffers(int n, uint[] buffers);
        internal static glGenBuffers _glGenBuffers;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenBuffers", IsExtension=false)]
        public static void GenBuffers(int n, uint[] buffers)
        {
            _glGenBuffers(n, buffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsBuffer(uint buffer);
        internal static glIsBuffer _glIsBuffer;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsBuffer", IsExtension=false)]
        public static bool IsBuffer(uint buffer)
        {
            bool data = _glIsBuffer(buffer);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBufferData(uint target, IntPtr size, IntPtr data, uint usage);
        internal static glBufferData _glBufferData;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBufferData", IsExtension=false)]
        public static void BufferData(uint target, IntPtr size, IntPtr data, uint usage)
        {
            _glBufferData(target, size, data, usage);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBufferSubData(uint target, IntPtr offset, IntPtr size, IntPtr data);
        internal static glBufferSubData _glBufferSubData;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBufferSubData", IsExtension=false)]
        public static void BufferSubData(uint target, IntPtr offset, IntPtr size, IntPtr data)
        {
            _glBufferSubData(target, offset, size, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetBufferSubData(uint target, IntPtr offset, IntPtr size, IntPtr data);
        internal static glGetBufferSubData _glGetBufferSubData;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetBufferSubData", IsExtension=false)]
        public static void GetBufferSubData(uint target, IntPtr offset, IntPtr size, IntPtr data)
        {
            _glGetBufferSubData(target, offset, size, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glMapBuffer(uint target, uint access);
        internal static glMapBuffer _glMapBuffer;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMapBuffer", IsExtension=false)]
        public static IntPtr MapBuffer(uint target, uint access)
        {
            IntPtr data = _glMapBuffer(target, access);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glUnmapBuffer(uint target);
        internal static glUnmapBuffer _glUnmapBuffer;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUnmapBuffer", IsExtension=false)]
        public static bool UnmapBuffer(uint target)
        {
            bool data = _glUnmapBuffer(target);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetBufferParameteriv(uint target, uint pname, int[] @params);
        internal static glGetBufferParameteriv _glGetBufferParameteriv;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetBufferParameteriv", IsExtension=false)]
        public static void GetBufferParameteriv(uint target, uint pname, int[] @params)
        {
            _glGetBufferParameteriv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetBufferPointerv(uint target, uint pname, IntPtr @params);
        internal static glGetBufferPointerv _glGetBufferPointerv;

        [Version(Group="GL_VERSION_1_5", Version = "1.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetBufferPointerv", IsExtension=false)]
        public static void GetBufferPointerv(uint target, uint pname, IntPtr @params)
        {
            _glGetBufferPointerv(target, pname, @params);
        }
        

        #endregion

        #region GL_VERSION_2_0
        public const uint GL_BLEND_EQUATION_RGB = (uint)0x8009;
        public const uint GL_VERTEX_ATTRIB_ARRAY_ENABLED = (uint)0x8622;
        public const uint GL_VERTEX_ATTRIB_ARRAY_SIZE = (uint)0x8623;
        public const uint GL_VERTEX_ATTRIB_ARRAY_STRIDE = (uint)0x8624;
        public const uint GL_VERTEX_ATTRIB_ARRAY_TYPE = (uint)0x8625;
        public const uint GL_CURRENT_VERTEX_ATTRIB = (uint)0x8626;
        public const uint GL_VERTEX_PROGRAM_POINT_SIZE = (uint)0x8642;
        public const uint GL_VERTEX_ATTRIB_ARRAY_POINTER = (uint)0x8645;
        public const uint GL_STENCIL_BACK_FUNC = (uint)0x8800;
        public const uint GL_STENCIL_BACK_FAIL = (uint)0x8801;
        public const uint GL_STENCIL_BACK_PASS_DEPTH_FAIL = (uint)0x8802;
        public const uint GL_STENCIL_BACK_PASS_DEPTH_PASS = (uint)0x8803;
        public const uint GL_MAX_DRAW_BUFFERS = (uint)0x8824;
        public const uint GL_DRAW_BUFFER0 = (uint)0x8825;
        public const uint GL_DRAW_BUFFER1 = (uint)0x8826;
        public const uint GL_DRAW_BUFFER2 = (uint)0x8827;
        public const uint GL_DRAW_BUFFER3 = (uint)0x8828;
        public const uint GL_DRAW_BUFFER4 = (uint)0x8829;
        public const uint GL_DRAW_BUFFER5 = (uint)0x882A;
        public const uint GL_DRAW_BUFFER6 = (uint)0x882B;
        public const uint GL_DRAW_BUFFER7 = (uint)0x882C;
        public const uint GL_DRAW_BUFFER8 = (uint)0x882D;
        public const uint GL_DRAW_BUFFER9 = (uint)0x882E;
        public const uint GL_DRAW_BUFFER10 = (uint)0x882F;
        public const uint GL_DRAW_BUFFER11 = (uint)0x8830;
        public const uint GL_DRAW_BUFFER12 = (uint)0x8831;
        public const uint GL_DRAW_BUFFER13 = (uint)0x8832;
        public const uint GL_DRAW_BUFFER14 = (uint)0x8833;
        public const uint GL_DRAW_BUFFER15 = (uint)0x8834;
        public const uint GL_BLEND_EQUATION_ALPHA = (uint)0x883D;
        public const uint GL_MAX_VERTEX_ATTRIBS = (uint)0x8869;
        public const uint GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = (uint)0x886A;
        public const uint GL_MAX_TEXTURE_IMAGE_UNITS = (uint)0x8872;
        public const uint GL_FRAGMENT_SHADER = (uint)0x8B30;
        public const uint GL_VERTEX_SHADER = (uint)0x8B31;
        public const uint GL_MAX_FRAGMENT_UNIFORM_COMPONENTS = (uint)0x8B49;
        public const uint GL_MAX_VERTEX_UNIFORM_COMPONENTS = (uint)0x8B4A;
        public const uint GL_MAX_VARYING_FLOATS = (uint)0x8B4B;
        public const uint GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = (uint)0x8B4C;
        public const uint GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = (uint)0x8B4D;
        public const uint GL_SHADER_TYPE = (uint)0x8B4F;
        public const uint GL_FLOAT_VEC2 = (uint)0x8B50;
        public const uint GL_FLOAT_VEC3 = (uint)0x8B51;
        public const uint GL_FLOAT_VEC4 = (uint)0x8B52;
        public const uint GL_INT_VEC2 = (uint)0x8B53;
        public const uint GL_INT_VEC3 = (uint)0x8B54;
        public const uint GL_INT_VEC4 = (uint)0x8B55;
        public const uint GL_BOOL = (uint)0x8B56;
        public const uint GL_BOOL_VEC2 = (uint)0x8B57;
        public const uint GL_BOOL_VEC3 = (uint)0x8B58;
        public const uint GL_BOOL_VEC4 = (uint)0x8B59;
        public const uint GL_FLOAT_MAT2 = (uint)0x8B5A;
        public const uint GL_FLOAT_MAT3 = (uint)0x8B5B;
        public const uint GL_FLOAT_MAT4 = (uint)0x8B5C;
        public const uint GL_SAMPLER_1D = (uint)0x8B5D;
        public const uint GL_SAMPLER_2D = (uint)0x8B5E;
        public const uint GL_SAMPLER_3D = (uint)0x8B5F;
        public const uint GL_SAMPLER_CUBE = (uint)0x8B60;
        public const uint GL_SAMPLER_1D_SHADOW = (uint)0x8B61;
        public const uint GL_SAMPLER_2D_SHADOW = (uint)0x8B62;
        public const uint GL_DELETE_STATUS = (uint)0x8B80;
        public const uint GL_COMPILE_STATUS = (uint)0x8B81;
        public const uint GL_LINK_STATUS = (uint)0x8B82;
        public const uint GL_VALIDATE_STATUS = (uint)0x8B83;
        public const uint GL_INFO_LOG_LENGTH = (uint)0x8B84;
        public const uint GL_ATTACHED_SHADERS = (uint)0x8B85;
        public const uint GL_ACTIVE_UNIFORMS = (uint)0x8B86;
        public const uint GL_ACTIVE_UNIFORM_MAX_LENGTH = (uint)0x8B87;
        public const uint GL_SHADER_SOURCE_LENGTH = (uint)0x8B88;
        public const uint GL_ACTIVE_ATTRIBUTES = (uint)0x8B89;
        public const uint GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = (uint)0x8B8A;
        public const uint GL_FRAGMENT_SHADER_DERIVATIVE_HINT = (uint)0x8B8B;
        public const uint GL_SHADING_LANGUAGE_VERSION = (uint)0x8B8C;
        public const uint GL_CURRENT_PROGRAM = (uint)0x8B8D;
        public const uint GL_POINT_SPRITE_COORD_ORIGIN = (uint)0x8CA0;
        public const uint GL_STENCIL_BACK_REF = (uint)0x8CA3;
        public const uint GL_STENCIL_BACK_VALUE_MASK = (uint)0x8CA4;
        public const uint GL_STENCIL_BACK_WRITEMASK = (uint)0x8CA5;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendEquationSeparate(uint modeRGB, uint modeAlpha);
        internal static glBlendEquationSeparate _glBlendEquationSeparate;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendEquationSeparate", IsExtension=false)]
        public static void BlendEquationSeparate(uint modeRGB, uint modeAlpha)
        {
            _glBlendEquationSeparate(modeRGB, modeAlpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawBuffers(int n, uint[] bufs);
        internal static glDrawBuffers _glDrawBuffers;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawBuffers", IsExtension=false)]
        public static void DrawBuffers(int n, uint[] bufs)
        {
            _glDrawBuffers(n, bufs);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass);
        internal static glStencilOpSeparate _glStencilOpSeparate;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilOpSeparate", IsExtension=false)]
        public static void StencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass)
        {
            _glStencilOpSeparate(face, sfail, dpfail, dppass);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilFuncSeparate(uint face, uint func, int @ref, uint mask);
        internal static glStencilFuncSeparate _glStencilFuncSeparate;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilFuncSeparate", IsExtension=false)]
        public static void StencilFuncSeparate(uint face, uint func, int @ref, uint mask)
        {
            _glStencilFuncSeparate(face, func, @ref, mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilMaskSeparate(uint face, uint mask);
        internal static glStencilMaskSeparate _glStencilMaskSeparate;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilMaskSeparate", IsExtension=false)]
        public static void StencilMaskSeparate(uint face, uint mask)
        {
            _glStencilMaskSeparate(face, mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glAttachShader(uint program, uint shader);
        internal static glAttachShader _glAttachShader;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glAttachShader", IsExtension=false)]
        public static void AttachShader(uint program, uint shader)
        {
            _glAttachShader(program, shader);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindAttribLocation(uint program, uint index, IntPtr name);
        internal static glBindAttribLocation _glBindAttribLocation;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindAttribLocation", IsExtension=false)]
        public static void BindAttribLocation(uint program, uint index, IntPtr name)
        {
            _glBindAttribLocation(program, index, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompileShader(uint shader);
        internal static glCompileShader _glCompileShader;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompileShader", IsExtension=false)]
        public static void CompileShader(uint shader)
        {
            _glCompileShader(shader);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glCreateProgram();
        internal static glCreateProgram _glCreateProgram;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateProgram", IsExtension=false)]
        public static uint CreateProgram()
        {
            uint data = _glCreateProgram();
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glCreateShader(uint type);
        internal static glCreateShader _glCreateShader;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateShader", IsExtension=false)]
        public static uint CreateShader(uint type)
        {
            uint data = _glCreateShader(type);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteProgram(uint program);
        internal static glDeleteProgram _glDeleteProgram;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteProgram", IsExtension=false)]
        public static void DeleteProgram(uint program)
        {
            _glDeleteProgram(program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteShader(uint shader);
        internal static glDeleteShader _glDeleteShader;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteShader", IsExtension=false)]
        public static void DeleteShader(uint shader)
        {
            _glDeleteShader(shader);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDetachShader(uint program, uint shader);
        internal static glDetachShader _glDetachShader;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDetachShader", IsExtension=false)]
        public static void DetachShader(uint program, uint shader)
        {
            _glDetachShader(program, shader);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDisableVertexAttribArray(uint index);
        internal static glDisableVertexAttribArray _glDisableVertexAttribArray;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDisableVertexAttribArray", IsExtension=false)]
        public static void DisableVertexAttribArray(uint index)
        {
            _glDisableVertexAttribArray(index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEnableVertexAttribArray(uint index);
        internal static glEnableVertexAttribArray _glEnableVertexAttribArray;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEnableVertexAttribArray", IsExtension=false)]
        public static void EnableVertexAttribArray(uint index)
        {
            _glEnableVertexAttribArray(index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveAttrib(uint program, uint index, int bufSize, IntPtr length, IntPtr size, IntPtr type, byte[] name);
        internal static glGetActiveAttrib _glGetActiveAttrib;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveAttrib", IsExtension=false)]
        public static void GetActiveAttrib(uint program, uint index, int bufSize, IntPtr length, IntPtr size, IntPtr type, byte[] name)
        {
            _glGetActiveAttrib(program, index, bufSize, length, size, type, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveUniform(uint program, uint index, int bufSize, IntPtr length, IntPtr size, IntPtr type, byte[] name);
        internal static glGetActiveUniform _glGetActiveUniform;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveUniform", IsExtension=false)]
        public static void GetActiveUniform(uint program, uint index, int bufSize, IntPtr length, IntPtr size, IntPtr type, byte[] name)
        {
            _glGetActiveUniform(program, index, bufSize, length, size, type, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetAttachedShaders(uint program, int maxCount, IntPtr count, uint[] shaders);
        internal static glGetAttachedShaders _glGetAttachedShaders;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetAttachedShaders", IsExtension=false)]
        public static void GetAttachedShaders(uint program, int maxCount, IntPtr count, uint[] shaders)
        {
            _glGetAttachedShaders(program, maxCount, count, shaders);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate int glGetAttribLocation(uint program, IntPtr name);
        internal static glGetAttribLocation _glGetAttribLocation;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetAttribLocation", IsExtension=false)]
        public static int GetAttribLocation(uint program, IntPtr name)
        {
            int data = _glGetAttribLocation(program, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramiv(uint program, uint pname, int[] @params);
        internal static glGetProgramiv _glGetProgramiv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramiv", IsExtension=false)]
        public static void GetProgramiv(uint program, uint pname, int[] @params)
        {
            _glGetProgramiv(program, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramInfoLog(uint program, int bufSize, IntPtr length, byte[] infoLog);
        internal static glGetProgramInfoLog _glGetProgramInfoLog;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramInfoLog", IsExtension=false)]
        public static void GetProgramInfoLog(uint program, int bufSize, IntPtr length, byte[] infoLog)
        {
            _glGetProgramInfoLog(program, bufSize, length, infoLog);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetShaderiv(uint shader, uint pname, int[] @params);
        internal static glGetShaderiv _glGetShaderiv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetShaderiv", IsExtension=false)]
        public static void GetShaderiv(uint shader, uint pname, int[] @params)
        {
            _glGetShaderiv(shader, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetShaderInfoLog(uint shader, int bufSize, IntPtr length, byte[] infoLog);
        internal static glGetShaderInfoLog _glGetShaderInfoLog;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetShaderInfoLog", IsExtension=false)]
        public static void GetShaderInfoLog(uint shader, int bufSize, IntPtr length, byte[] infoLog)
        {
            _glGetShaderInfoLog(shader, bufSize, length, infoLog);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetShaderSource(uint shader, int bufSize, IntPtr length, byte[] source);
        internal static glGetShaderSource _glGetShaderSource;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetShaderSource", IsExtension=false)]
        public static void GetShaderSource(uint shader, int bufSize, IntPtr length, byte[] source)
        {
            _glGetShaderSource(shader, bufSize, length, source);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate int glGetUniformLocation(uint program, IntPtr name);
        internal static glGetUniformLocation _glGetUniformLocation;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformLocation", IsExtension=false)]
        public static int GetUniformLocation(uint program, IntPtr name)
        {
            int data = _glGetUniformLocation(program, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetUniformfv(uint program, int location, float[] @params);
        internal static glGetUniformfv _glGetUniformfv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformfv", IsExtension=false)]
        public static void GetUniformfv(uint program, int location, float[] @params)
        {
            _glGetUniformfv(program, location, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetUniformiv(uint program, int location, int[] @params);
        internal static glGetUniformiv _glGetUniformiv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformiv", IsExtension=false)]
        public static void GetUniformiv(uint program, int location, int[] @params)
        {
            _glGetUniformiv(program, location, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribdv(uint index, uint pname, double[] @params);
        internal static glGetVertexAttribdv _glGetVertexAttribdv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribdv", IsExtension=false)]
        public static void GetVertexAttribdv(uint index, uint pname, double[] @params)
        {
            _glGetVertexAttribdv(index, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribfv(uint index, uint pname, float[] @params);
        internal static glGetVertexAttribfv _glGetVertexAttribfv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribfv", IsExtension=false)]
        public static void GetVertexAttribfv(uint index, uint pname, float[] @params)
        {
            _glGetVertexAttribfv(index, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribiv(uint index, uint pname, int[] @params);
        internal static glGetVertexAttribiv _glGetVertexAttribiv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribiv", IsExtension=false)]
        public static void GetVertexAttribiv(uint index, uint pname, int[] @params)
        {
            _glGetVertexAttribiv(index, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribPointerv(uint index, uint pname, IntPtr pointer);
        internal static glGetVertexAttribPointerv _glGetVertexAttribPointerv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribPointerv", IsExtension=false)]
        public static void GetVertexAttribPointerv(uint index, uint pname, IntPtr pointer)
        {
            _glGetVertexAttribPointerv(index, pname, pointer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsProgram(uint program);
        internal static glIsProgram _glIsProgram;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsProgram", IsExtension=false)]
        public static bool IsProgram(uint program)
        {
            bool data = _glIsProgram(program);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsShader(uint shader);
        internal static glIsShader _glIsShader;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsShader", IsExtension=false)]
        public static bool IsShader(uint shader)
        {
            bool data = _glIsShader(shader);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glLinkProgram(uint program);
        internal static glLinkProgram _glLinkProgram;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glLinkProgram", IsExtension=false)]
        public static void LinkProgram(uint program)
        {
            _glLinkProgram(program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glShaderSource(uint shader, int count, string[] @string, int[] length);
        internal static glShaderSource _glShaderSource;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glShaderSource", IsExtension=false)]
        public static void ShaderSource(uint shader, int count, string[] @string, int[] length)
        {
            _glShaderSource(shader, count, @string, length);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUseProgram(uint program);
        internal static glUseProgram _glUseProgram;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUseProgram", IsExtension=false)]
        public static void UseProgram(uint program)
        {
            _glUseProgram(program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1f(int location, float v0);
        internal static glUniform1f _glUniform1f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1f", IsExtension=false)]
        public static void Uniform1f(int location, float v0)
        {
            _glUniform1f(location, v0);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2f(int location, float v0, float v1);
        internal static glUniform2f _glUniform2f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2f", IsExtension=false)]
        public static void Uniform2f(int location, float v0, float v1)
        {
            _glUniform2f(location, v0, v1);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3f(int location, float v0, float v1, float v2);
        internal static glUniform3f _glUniform3f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3f", IsExtension=false)]
        public static void Uniform3f(int location, float v0, float v1, float v2)
        {
            _glUniform3f(location, v0, v1, v2);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4f(int location, float v0, float v1, float v2, float v3);
        internal static glUniform4f _glUniform4f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4f", IsExtension=false)]
        public static void Uniform4f(int location, float v0, float v1, float v2, float v3)
        {
            _glUniform4f(location, v0, v1, v2, v3);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1i(int location, int v0);
        internal static glUniform1i _glUniform1i;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1i", IsExtension=false)]
        public static void Uniform1i(int location, int v0)
        {
            _glUniform1i(location, v0);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2i(int location, int v0, int v1);
        internal static glUniform2i _glUniform2i;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2i", IsExtension=false)]
        public static void Uniform2i(int location, int v0, int v1)
        {
            _glUniform2i(location, v0, v1);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3i(int location, int v0, int v1, int v2);
        internal static glUniform3i _glUniform3i;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3i", IsExtension=false)]
        public static void Uniform3i(int location, int v0, int v1, int v2)
        {
            _glUniform3i(location, v0, v1, v2);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4i(int location, int v0, int v1, int v2, int v3);
        internal static glUniform4i _glUniform4i;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4i", IsExtension=false)]
        public static void Uniform4i(int location, int v0, int v1, int v2, int v3)
        {
            _glUniform4i(location, v0, v1, v2, v3);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1fv(int location, int count, float[] value);
        internal static glUniform1fv _glUniform1fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1fv", IsExtension=false)]
        public static void Uniform1fv(int location, int count, float[] value)
        {
            _glUniform1fv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2fv(int location, int count, float[] value);
        internal static glUniform2fv _glUniform2fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2fv", IsExtension=false)]
        public static void Uniform2fv(int location, int count, float[] value)
        {
            _glUniform2fv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3fv(int location, int count, float[] value);
        internal static glUniform3fv _glUniform3fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3fv", IsExtension=false)]
        public static void Uniform3fv(int location, int count, float[] value)
        {
            _glUniform3fv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4fv(int location, int count, float[] value);
        internal static glUniform4fv _glUniform4fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4fv", IsExtension=false)]
        public static void Uniform4fv(int location, int count, float[] value)
        {
            _glUniform4fv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1iv(int location, int count, int[] value);
        internal static glUniform1iv _glUniform1iv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1iv", IsExtension=false)]
        public static void Uniform1iv(int location, int count, int[] value)
        {
            _glUniform1iv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2iv(int location, int count, int[] value);
        internal static glUniform2iv _glUniform2iv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2iv", IsExtension=false)]
        public static void Uniform2iv(int location, int count, int[] value)
        {
            _glUniform2iv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3iv(int location, int count, int[] value);
        internal static glUniform3iv _glUniform3iv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3iv", IsExtension=false)]
        public static void Uniform3iv(int location, int count, int[] value)
        {
            _glUniform3iv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4iv(int location, int count, int[] value);
        internal static glUniform4iv _glUniform4iv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4iv", IsExtension=false)]
        public static void Uniform4iv(int location, int count, int[] value)
        {
            _glUniform4iv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix2fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix2fv _glUniformMatrix2fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix2fv", IsExtension=false)]
        public static void UniformMatrix2fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix2fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix3fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix3fv _glUniformMatrix3fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix3fv", IsExtension=false)]
        public static void UniformMatrix3fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix3fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix4fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix4fv _glUniformMatrix4fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix4fv", IsExtension=false)]
        public static void UniformMatrix4fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix4fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glValidateProgram(uint program);
        internal static glValidateProgram _glValidateProgram;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glValidateProgram", IsExtension=false)]
        public static void ValidateProgram(uint program)
        {
            _glValidateProgram(program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib1d(uint index, double x);
        internal static glVertexAttrib1d _glVertexAttrib1d;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib1d", IsExtension=false)]
        public static void VertexAttrib1d(uint index, double x)
        {
            _glVertexAttrib1d(index, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib1dv(uint index, IntPtr v);
        internal static glVertexAttrib1dv _glVertexAttrib1dv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib1dv", IsExtension=false)]
        public static void VertexAttrib1dv(uint index, IntPtr v)
        {
            _glVertexAttrib1dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib1f(uint index, float x);
        internal static glVertexAttrib1f _glVertexAttrib1f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib1f", IsExtension=false)]
        public static void VertexAttrib1f(uint index, float x)
        {
            _glVertexAttrib1f(index, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib1fv(uint index, IntPtr v);
        internal static glVertexAttrib1fv _glVertexAttrib1fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib1fv", IsExtension=false)]
        public static void VertexAttrib1fv(uint index, IntPtr v)
        {
            _glVertexAttrib1fv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib1s(uint index, short x);
        internal static glVertexAttrib1s _glVertexAttrib1s;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib1s", IsExtension=false)]
        public static void VertexAttrib1s(uint index, short x)
        {
            _glVertexAttrib1s(index, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib1sv(uint index, IntPtr v);
        internal static glVertexAttrib1sv _glVertexAttrib1sv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib1sv", IsExtension=false)]
        public static void VertexAttrib1sv(uint index, IntPtr v)
        {
            _glVertexAttrib1sv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib2d(uint index, double x, double y);
        internal static glVertexAttrib2d _glVertexAttrib2d;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib2d", IsExtension=false)]
        public static void VertexAttrib2d(uint index, double x, double y)
        {
            _glVertexAttrib2d(index, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib2dv(uint index, double[] v);
        internal static glVertexAttrib2dv _glVertexAttrib2dv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib2dv", IsExtension=false)]
        public static void VertexAttrib2dv(uint index, double[] v)
        {
            _glVertexAttrib2dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib2f(uint index, float x, float y);
        internal static glVertexAttrib2f _glVertexAttrib2f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib2f", IsExtension=false)]
        public static void VertexAttrib2f(uint index, float x, float y)
        {
            _glVertexAttrib2f(index, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib2fv(uint index, float[] v);
        internal static glVertexAttrib2fv _glVertexAttrib2fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib2fv", IsExtension=false)]
        public static void VertexAttrib2fv(uint index, float[] v)
        {
            _glVertexAttrib2fv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib2s(uint index, short x, short y);
        internal static glVertexAttrib2s _glVertexAttrib2s;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib2s", IsExtension=false)]
        public static void VertexAttrib2s(uint index, short x, short y)
        {
            _glVertexAttrib2s(index, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib2sv(uint index, short[] v);
        internal static glVertexAttrib2sv _glVertexAttrib2sv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib2sv", IsExtension=false)]
        public static void VertexAttrib2sv(uint index, short[] v)
        {
            _glVertexAttrib2sv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib3d(uint index, double x, double y, double z);
        internal static glVertexAttrib3d _glVertexAttrib3d;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib3d", IsExtension=false)]
        public static void VertexAttrib3d(uint index, double x, double y, double z)
        {
            _glVertexAttrib3d(index, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib3dv(uint index, double[] v);
        internal static glVertexAttrib3dv _glVertexAttrib3dv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib3dv", IsExtension=false)]
        public static void VertexAttrib3dv(uint index, double[] v)
        {
            _glVertexAttrib3dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib3f(uint index, float x, float y, float z);
        internal static glVertexAttrib3f _glVertexAttrib3f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib3f", IsExtension=false)]
        public static void VertexAttrib3f(uint index, float x, float y, float z)
        {
            _glVertexAttrib3f(index, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib3fv(uint index, float[] v);
        internal static glVertexAttrib3fv _glVertexAttrib3fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib3fv", IsExtension=false)]
        public static void VertexAttrib3fv(uint index, float[] v)
        {
            _glVertexAttrib3fv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib3s(uint index, short x, short y, short z);
        internal static glVertexAttrib3s _glVertexAttrib3s;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib3s", IsExtension=false)]
        public static void VertexAttrib3s(uint index, short x, short y, short z)
        {
            _glVertexAttrib3s(index, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib3sv(uint index, short[] v);
        internal static glVertexAttrib3sv _glVertexAttrib3sv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib3sv", IsExtension=false)]
        public static void VertexAttrib3sv(uint index, short[] v)
        {
            _glVertexAttrib3sv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4Nbv(uint index, sbyte[] v);
        internal static glVertexAttrib4Nbv _glVertexAttrib4Nbv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4Nbv", IsExtension=false)]
        public static void VertexAttrib4Nbv(uint index, sbyte[] v)
        {
            _glVertexAttrib4Nbv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4Niv(uint index, int[] v);
        internal static glVertexAttrib4Niv _glVertexAttrib4Niv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4Niv", IsExtension=false)]
        public static void VertexAttrib4Niv(uint index, int[] v)
        {
            _glVertexAttrib4Niv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4Nsv(uint index, short[] v);
        internal static glVertexAttrib4Nsv _glVertexAttrib4Nsv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4Nsv", IsExtension=false)]
        public static void VertexAttrib4Nsv(uint index, short[] v)
        {
            _glVertexAttrib4Nsv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4Nub(uint index, byte x, byte y, byte z, byte w);
        internal static glVertexAttrib4Nub _glVertexAttrib4Nub;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4Nub", IsExtension=false)]
        public static void VertexAttrib4Nub(uint index, byte x, byte y, byte z, byte w)
        {
            _glVertexAttrib4Nub(index, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4Nubv(uint index, string v);
        internal static glVertexAttrib4Nubv _glVertexAttrib4Nubv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4Nubv", IsExtension=false)]
        public static void VertexAttrib4Nubv(uint index, string v)
        {
            _glVertexAttrib4Nubv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4Nuiv(uint index, uint[] v);
        internal static glVertexAttrib4Nuiv _glVertexAttrib4Nuiv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4Nuiv", IsExtension=false)]
        public static void VertexAttrib4Nuiv(uint index, uint[] v)
        {
            _glVertexAttrib4Nuiv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4Nusv(uint index, ushort[] v);
        internal static glVertexAttrib4Nusv _glVertexAttrib4Nusv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4Nusv", IsExtension=false)]
        public static void VertexAttrib4Nusv(uint index, ushort[] v)
        {
            _glVertexAttrib4Nusv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4bv(uint index, sbyte[] v);
        internal static glVertexAttrib4bv _glVertexAttrib4bv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4bv", IsExtension=false)]
        public static void VertexAttrib4bv(uint index, sbyte[] v)
        {
            _glVertexAttrib4bv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4d(uint index, double x, double y, double z, double w);
        internal static glVertexAttrib4d _glVertexAttrib4d;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4d", IsExtension=false)]
        public static void VertexAttrib4d(uint index, double x, double y, double z, double w)
        {
            _glVertexAttrib4d(index, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4dv(uint index, double[] v);
        internal static glVertexAttrib4dv _glVertexAttrib4dv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4dv", IsExtension=false)]
        public static void VertexAttrib4dv(uint index, double[] v)
        {
            _glVertexAttrib4dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4f(uint index, float x, float y, float z, float w);
        internal static glVertexAttrib4f _glVertexAttrib4f;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4f", IsExtension=false)]
        public static void VertexAttrib4f(uint index, float x, float y, float z, float w)
        {
            _glVertexAttrib4f(index, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4fv(uint index, float[] v);
        internal static glVertexAttrib4fv _glVertexAttrib4fv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4fv", IsExtension=false)]
        public static void VertexAttrib4fv(uint index, float[] v)
        {
            _glVertexAttrib4fv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4iv(uint index, int[] v);
        internal static glVertexAttrib4iv _glVertexAttrib4iv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4iv", IsExtension=false)]
        public static void VertexAttrib4iv(uint index, int[] v)
        {
            _glVertexAttrib4iv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4s(uint index, short x, short y, short z, short w);
        internal static glVertexAttrib4s _glVertexAttrib4s;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4s", IsExtension=false)]
        public static void VertexAttrib4s(uint index, short x, short y, short z, short w)
        {
            _glVertexAttrib4s(index, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4sv(uint index, short[] v);
        internal static glVertexAttrib4sv _glVertexAttrib4sv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4sv", IsExtension=false)]
        public static void VertexAttrib4sv(uint index, short[] v)
        {
            _glVertexAttrib4sv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4ubv(uint index, string v);
        internal static glVertexAttrib4ubv _glVertexAttrib4ubv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4ubv", IsExtension=false)]
        public static void VertexAttrib4ubv(uint index, string v)
        {
            _glVertexAttrib4ubv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4uiv(uint index, uint[] v);
        internal static glVertexAttrib4uiv _glVertexAttrib4uiv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4uiv", IsExtension=false)]
        public static void VertexAttrib4uiv(uint index, uint[] v)
        {
            _glVertexAttrib4uiv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttrib4usv(uint index, ushort[] v);
        internal static glVertexAttrib4usv _glVertexAttrib4usv;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttrib4usv", IsExtension=false)]
        public static void VertexAttrib4usv(uint index, ushort[] v)
        {
            _glVertexAttrib4usv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);
        internal static glVertexAttribPointer _glVertexAttribPointer;

        [Version(Group="GL_VERSION_2_0", Version = "2.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribPointer", IsExtension=false)]
        public static void VertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer)
        {
            _glVertexAttribPointer(index, size, type, normalized, stride, pointer);
        }
        

        #endregion

        #region GL_VERSION_2_1
        public const uint GL_PIXEL_PACK_BUFFER = (uint)0x88EB;
        public const uint GL_PIXEL_UNPACK_BUFFER = (uint)0x88EC;
        public const uint GL_PIXEL_PACK_BUFFER_BINDING = (uint)0x88ED;
        public const uint GL_PIXEL_UNPACK_BUFFER_BINDING = (uint)0x88EF;
        public const uint GL_FLOAT_MAT2x3 = (uint)0x8B65;
        public const uint GL_FLOAT_MAT2x4 = (uint)0x8B66;
        public const uint GL_FLOAT_MAT3x2 = (uint)0x8B67;
        public const uint GL_FLOAT_MAT3x4 = (uint)0x8B68;
        public const uint GL_FLOAT_MAT4x2 = (uint)0x8B69;
        public const uint GL_FLOAT_MAT4x3 = (uint)0x8B6A;
        public const uint GL_SRGB = (uint)0x8C40;
        public const uint GL_SRGB8 = (uint)0x8C41;
        public const uint GL_SRGB_ALPHA = (uint)0x8C42;
        public const uint GL_SRGB8_ALPHA8 = (uint)0x8C43;
        public const uint GL_COMPRESSED_SRGB = (uint)0x8C48;
        public const uint GL_COMPRESSED_SRGB_ALPHA = (uint)0x8C49;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix2x3fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix2x3fv _glUniformMatrix2x3fv;

        [Version(Group="GL_VERSION_2_1", Version = "2.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix2x3fv", IsExtension=false)]
        public static void UniformMatrix2x3fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix2x3fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix3x2fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix3x2fv _glUniformMatrix3x2fv;

        [Version(Group="GL_VERSION_2_1", Version = "2.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix3x2fv", IsExtension=false)]
        public static void UniformMatrix3x2fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix3x2fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix2x4fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix2x4fv _glUniformMatrix2x4fv;

        [Version(Group="GL_VERSION_2_1", Version = "2.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix2x4fv", IsExtension=false)]
        public static void UniformMatrix2x4fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix2x4fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix4x2fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix4x2fv _glUniformMatrix4x2fv;

        [Version(Group="GL_VERSION_2_1", Version = "2.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix4x2fv", IsExtension=false)]
        public static void UniformMatrix4x2fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix4x2fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix3x4fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix3x4fv _glUniformMatrix3x4fv;

        [Version(Group="GL_VERSION_2_1", Version = "2.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix3x4fv", IsExtension=false)]
        public static void UniformMatrix3x4fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix3x4fv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix4x3fv(int location, int count, bool transpose, float[] value);
        internal static glUniformMatrix4x3fv _glUniformMatrix4x3fv;

        [Version(Group="GL_VERSION_2_1", Version = "2.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix4x3fv", IsExtension=false)]
        public static void UniformMatrix4x3fv(int location, int count, bool transpose, float[] value)
        {
            _glUniformMatrix4x3fv(location, count, transpose, value);
        }
        

        #endregion

        #region GL_VERSION_3_0
        public const uint GL_COMPARE_REF_TO_TEXTURE = (uint)0x884E;
        public const uint GL_CLIP_DISTANCE0 = (uint)0x3000;
        public const uint GL_CLIP_DISTANCE1 = (uint)0x3001;
        public const uint GL_CLIP_DISTANCE2 = (uint)0x3002;
        public const uint GL_CLIP_DISTANCE3 = (uint)0x3003;
        public const uint GL_CLIP_DISTANCE4 = (uint)0x3004;
        public const uint GL_CLIP_DISTANCE5 = (uint)0x3005;
        public const uint GL_CLIP_DISTANCE6 = (uint)0x3006;
        public const uint GL_CLIP_DISTANCE7 = (uint)0x3007;
        public const uint GL_MAX_CLIP_DISTANCES = (uint)0x0D32;
        public const uint GL_MAJOR_VERSION = (uint)0x821B;
        public const uint GL_MINOR_VERSION = (uint)0x821C;
        public const uint GL_NUM_EXTENSIONS = (uint)0x821D;
        public const uint GL_CONTEXT_FLAGS = (uint)0x821E;
        public const uint GL_COMPRESSED_RED = (uint)0x8225;
        public const uint GL_COMPRESSED_RG = (uint)0x8226;
        public const uint GL_CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = (uint)0x00000001;
        public const uint GL_RGBA32F = (uint)0x8814;
        public const uint GL_RGB32F = (uint)0x8815;
        public const uint GL_RGBA16F = (uint)0x881A;
        public const uint GL_RGB16F = (uint)0x881B;
        public const uint GL_VERTEX_ATTRIB_ARRAY_INTEGER = (uint)0x88FD;
        public const uint GL_MAX_ARRAY_TEXTURE_LAYERS = (uint)0x88FF;
        public const uint GL_MIN_PROGRAM_TEXEL_OFFSET = (uint)0x8904;
        public const uint GL_MAX_PROGRAM_TEXEL_OFFSET = (uint)0x8905;
        public const uint GL_CLAMP_READ_COLOR = (uint)0x891C;
        public const uint GL_FIXED_ONLY = (uint)0x891D;
        public const uint GL_MAX_VARYING_COMPONENTS = (uint)0x8B4B;
        public const uint GL_TEXTURE_1D_ARRAY = (uint)0x8C18;
        public const uint GL_PROXY_TEXTURE_1D_ARRAY = (uint)0x8C19;
        public const uint GL_TEXTURE_2D_ARRAY = (uint)0x8C1A;
        public const uint GL_PROXY_TEXTURE_2D_ARRAY = (uint)0x8C1B;
        public const uint GL_R11F_G11F_B10F = (uint)0x8C3A;
        public const uint GL_RGB9_E5 = (uint)0x8C3D;
        public const uint GL_UNSIGNED_INT_5_9_9_9_REV = (uint)0x8C3E;
        public const uint GL_TEXTURE_SHARED_SIZE = (uint)0x8C3F;
        public const uint GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = (uint)0x8C76;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_MODE = (uint)0x8C7F;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = (uint)0x8C80;
        public const uint GL_TRANSFORM_FEEDBACK_VARYINGS = (uint)0x8C83;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_START = (uint)0x8C84;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_SIZE = (uint)0x8C85;
        public const uint GL_PRIMITIVES_GENERATED = (uint)0x8C87;
        public const uint GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = (uint)0x8C88;
        public const uint GL_RASTERIZER_DISCARD = (uint)0x8C89;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = (uint)0x8C8A;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = (uint)0x8C8B;
        public const uint GL_INTERLEAVED_ATTRIBS = (uint)0x8C8C;
        public const uint GL_SEPARATE_ATTRIBS = (uint)0x8C8D;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_BINDING = (uint)0x8C8F;
        public const uint GL_RGBA32UI = (uint)0x8D70;
        public const uint GL_RGB32UI = (uint)0x8D71;
        public const uint GL_RGBA16UI = (uint)0x8D76;
        public const uint GL_RGB16UI = (uint)0x8D77;
        public const uint GL_RGBA8UI = (uint)0x8D7C;
        public const uint GL_RGB8UI = (uint)0x8D7D;
        public const uint GL_RGBA32I = (uint)0x8D82;
        public const uint GL_RGB32I = (uint)0x8D83;
        public const uint GL_RGBA16I = (uint)0x8D88;
        public const uint GL_RGB16I = (uint)0x8D89;
        public const uint GL_RGBA8I = (uint)0x8D8E;
        public const uint GL_RGB8I = (uint)0x8D8F;
        public const uint GL_RED_INTEGER = (uint)0x8D94;
        public const uint GL_GREEN_INTEGER = (uint)0x8D95;
        public const uint GL_BLUE_INTEGER = (uint)0x8D96;
        public const uint GL_RGB_INTEGER = (uint)0x8D98;
        public const uint GL_RGBA_INTEGER = (uint)0x8D99;
        public const uint GL_BGR_INTEGER = (uint)0x8D9A;
        public const uint GL_BGRA_INTEGER = (uint)0x8D9B;
        public const uint GL_SAMPLER_1D_ARRAY = (uint)0x8DC0;
        public const uint GL_SAMPLER_2D_ARRAY = (uint)0x8DC1;
        public const uint GL_SAMPLER_1D_ARRAY_SHADOW = (uint)0x8DC3;
        public const uint GL_SAMPLER_2D_ARRAY_SHADOW = (uint)0x8DC4;
        public const uint GL_SAMPLER_CUBE_SHADOW = (uint)0x8DC5;
        public const uint GL_UNSIGNED_INT_VEC2 = (uint)0x8DC6;
        public const uint GL_UNSIGNED_INT_VEC3 = (uint)0x8DC7;
        public const uint GL_UNSIGNED_INT_VEC4 = (uint)0x8DC8;
        public const uint GL_INT_SAMPLER_1D = (uint)0x8DC9;
        public const uint GL_INT_SAMPLER_2D = (uint)0x8DCA;
        public const uint GL_INT_SAMPLER_3D = (uint)0x8DCB;
        public const uint GL_INT_SAMPLER_CUBE = (uint)0x8DCC;
        public const uint GL_INT_SAMPLER_1D_ARRAY = (uint)0x8DCE;
        public const uint GL_INT_SAMPLER_2D_ARRAY = (uint)0x8DCF;
        public const uint GL_UNSIGNED_INT_SAMPLER_1D = (uint)0x8DD1;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D = (uint)0x8DD2;
        public const uint GL_UNSIGNED_INT_SAMPLER_3D = (uint)0x8DD3;
        public const uint GL_UNSIGNED_INT_SAMPLER_CUBE = (uint)0x8DD4;
        public const uint GL_UNSIGNED_INT_SAMPLER_1D_ARRAY = (uint)0x8DD6;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = (uint)0x8DD7;
        public const uint GL_QUERY_WAIT = (uint)0x8E13;
        public const uint GL_QUERY_NO_WAIT = (uint)0x8E14;
        public const uint GL_QUERY_BY_REGION_WAIT = (uint)0x8E15;
        public const uint GL_QUERY_BY_REGION_NO_WAIT = (uint)0x8E16;
        public const uint GL_BUFFER_ACCESS_FLAGS = (uint)0x911F;
        public const uint GL_BUFFER_MAP_LENGTH = (uint)0x9120;
        public const uint GL_BUFFER_MAP_OFFSET = (uint)0x9121;
        public const uint GL_DEPTH_COMPONENT32F = (uint)0x8CAC;
        public const uint GL_DEPTH32F_STENCIL8 = (uint)0x8CAD;
        public const uint GL_FLOAT_32_UNSIGNED_INT_24_8_REV = (uint)0x8DAD;
        public const uint GL_INVALID_FRAMEBUFFER_OPERATION = (uint)0x0506;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = (uint)0x8210;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = (uint)0x8211;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE = (uint)0x8212;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = (uint)0x8213;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = (uint)0x8214;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = (uint)0x8215;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = (uint)0x8216;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = (uint)0x8217;
        public const uint GL_FRAMEBUFFER_DEFAULT = (uint)0x8218;
        public const uint GL_FRAMEBUFFER_UNDEFINED = (uint)0x8219;
        public const uint GL_DEPTH_STENCIL_ATTACHMENT = (uint)0x821A;
        public const uint GL_MAX_RENDERBUFFER_SIZE = (uint)0x84E8;
        public const uint GL_DEPTH_STENCIL = (uint)0x84F9;
        public const uint GL_UNSIGNED_INT_24_8 = (uint)0x84FA;
        public const uint GL_DEPTH24_STENCIL8 = (uint)0x88F0;
        public const uint GL_TEXTURE_STENCIL_SIZE = (uint)0x88F1;
        public const uint GL_TEXTURE_RED_TYPE = (uint)0x8C10;
        public const uint GL_TEXTURE_GREEN_TYPE = (uint)0x8C11;
        public const uint GL_TEXTURE_BLUE_TYPE = (uint)0x8C12;
        public const uint GL_TEXTURE_ALPHA_TYPE = (uint)0x8C13;
        public const uint GL_TEXTURE_DEPTH_TYPE = (uint)0x8C16;
        public const uint GL_UNSIGNED_NORMALIZED = (uint)0x8C17;
        public const uint GL_FRAMEBUFFER_BINDING = (uint)0x8CA6;
        public const uint GL_DRAW_FRAMEBUFFER_BINDING = (uint)0x8CA6;
        public const uint GL_RENDERBUFFER_BINDING = (uint)0x8CA7;
        public const uint GL_READ_FRAMEBUFFER = (uint)0x8CA8;
        public const uint GL_DRAW_FRAMEBUFFER = (uint)0x8CA9;
        public const uint GL_READ_FRAMEBUFFER_BINDING = (uint)0x8CAA;
        public const uint GL_RENDERBUFFER_SAMPLES = (uint)0x8CAB;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = (uint)0x8CD0;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = (uint)0x8CD1;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = (uint)0x8CD2;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = (uint)0x8CD3;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = (uint)0x8CD4;
        public const uint GL_FRAMEBUFFER_COMPLETE = (uint)0x8CD5;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = (uint)0x8CD6;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = (uint)0x8CD7;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = (uint)0x8CDB;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER = (uint)0x8CDC;
        public const uint GL_FRAMEBUFFER_UNSUPPORTED = (uint)0x8CDD;
        public const uint GL_MAX_COLOR_ATTACHMENTS = (uint)0x8CDF;
        public const uint GL_COLOR_ATTACHMENT0 = (uint)0x8CE0;
        public const uint GL_COLOR_ATTACHMENT1 = (uint)0x8CE1;
        public const uint GL_COLOR_ATTACHMENT2 = (uint)0x8CE2;
        public const uint GL_COLOR_ATTACHMENT3 = (uint)0x8CE3;
        public const uint GL_COLOR_ATTACHMENT4 = (uint)0x8CE4;
        public const uint GL_COLOR_ATTACHMENT5 = (uint)0x8CE5;
        public const uint GL_COLOR_ATTACHMENT6 = (uint)0x8CE6;
        public const uint GL_COLOR_ATTACHMENT7 = (uint)0x8CE7;
        public const uint GL_COLOR_ATTACHMENT8 = (uint)0x8CE8;
        public const uint GL_COLOR_ATTACHMENT9 = (uint)0x8CE9;
        public const uint GL_COLOR_ATTACHMENT10 = (uint)0x8CEA;
        public const uint GL_COLOR_ATTACHMENT11 = (uint)0x8CEB;
        public const uint GL_COLOR_ATTACHMENT12 = (uint)0x8CEC;
        public const uint GL_COLOR_ATTACHMENT13 = (uint)0x8CED;
        public const uint GL_COLOR_ATTACHMENT14 = (uint)0x8CEE;
        public const uint GL_COLOR_ATTACHMENT15 = (uint)0x8CEF;
        public const uint GL_COLOR_ATTACHMENT16 = (uint)0x8CF0;
        public const uint GL_COLOR_ATTACHMENT17 = (uint)0x8CF1;
        public const uint GL_COLOR_ATTACHMENT18 = (uint)0x8CF2;
        public const uint GL_COLOR_ATTACHMENT19 = (uint)0x8CF3;
        public const uint GL_COLOR_ATTACHMENT20 = (uint)0x8CF4;
        public const uint GL_COLOR_ATTACHMENT21 = (uint)0x8CF5;
        public const uint GL_COLOR_ATTACHMENT22 = (uint)0x8CF6;
        public const uint GL_COLOR_ATTACHMENT23 = (uint)0x8CF7;
        public const uint GL_COLOR_ATTACHMENT24 = (uint)0x8CF8;
        public const uint GL_COLOR_ATTACHMENT25 = (uint)0x8CF9;
        public const uint GL_COLOR_ATTACHMENT26 = (uint)0x8CFA;
        public const uint GL_COLOR_ATTACHMENT27 = (uint)0x8CFB;
        public const uint GL_COLOR_ATTACHMENT28 = (uint)0x8CFC;
        public const uint GL_COLOR_ATTACHMENT29 = (uint)0x8CFD;
        public const uint GL_COLOR_ATTACHMENT30 = (uint)0x8CFE;
        public const uint GL_COLOR_ATTACHMENT31 = (uint)0x8CFF;
        public const uint GL_DEPTH_ATTACHMENT = (uint)0x8D00;
        public const uint GL_STENCIL_ATTACHMENT = (uint)0x8D20;
        public const uint GL_FRAMEBUFFER = (uint)0x8D40;
        public const uint GL_RENDERBUFFER = (uint)0x8D41;
        public const uint GL_RENDERBUFFER_WIDTH = (uint)0x8D42;
        public const uint GL_RENDERBUFFER_HEIGHT = (uint)0x8D43;
        public const uint GL_RENDERBUFFER_INTERNAL_FORMAT = (uint)0x8D44;
        public const uint GL_STENCIL_INDEX1 = (uint)0x8D46;
        public const uint GL_STENCIL_INDEX4 = (uint)0x8D47;
        public const uint GL_STENCIL_INDEX16 = (uint)0x8D49;
        public const uint GL_RENDERBUFFER_RED_SIZE = (uint)0x8D50;
        public const uint GL_RENDERBUFFER_GREEN_SIZE = (uint)0x8D51;
        public const uint GL_RENDERBUFFER_BLUE_SIZE = (uint)0x8D52;
        public const uint GL_RENDERBUFFER_ALPHA_SIZE = (uint)0x8D53;
        public const uint GL_RENDERBUFFER_DEPTH_SIZE = (uint)0x8D54;
        public const uint GL_RENDERBUFFER_STENCIL_SIZE = (uint)0x8D55;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = (uint)0x8D56;
        public const uint GL_MAX_SAMPLES = (uint)0x8D57;
        public const uint GL_FRAMEBUFFER_SRGB = (uint)0x8DB9;
        public const uint GL_HALF_FLOAT = (uint)0x140B;
        public const uint GL_MAP_INVALIDATE_RANGE_BIT = (uint)0x0004;
        public const uint GL_MAP_INVALIDATE_BUFFER_BIT = (uint)0x0008;
        public const uint GL_MAP_FLUSH_EXPLICIT_BIT = (uint)0x0010;
        public const uint GL_MAP_UNSYNCHRONIZED_BIT = (uint)0x0020;
        public const uint GL_COMPRESSED_RED_RGTC1 = (uint)0x8DBB;
        public const uint GL_COMPRESSED_SIGNED_RED_RGTC1 = (uint)0x8DBC;
        public const uint GL_COMPRESSED_RG_RGTC2 = (uint)0x8DBD;
        public const uint GL_COMPRESSED_SIGNED_RG_RGTC2 = (uint)0x8DBE;
        public const uint GL_RG = (uint)0x8227;
        public const uint GL_RG_INTEGER = (uint)0x8228;
        public const uint GL_R8 = (uint)0x8229;
        public const uint GL_R16 = (uint)0x822A;
        public const uint GL_RG8 = (uint)0x822B;
        public const uint GL_RG16 = (uint)0x822C;
        public const uint GL_R16F = (uint)0x822D;
        public const uint GL_R32F = (uint)0x822E;
        public const uint GL_RG16F = (uint)0x822F;
        public const uint GL_RG32F = (uint)0x8230;
        public const uint GL_R8I = (uint)0x8231;
        public const uint GL_R8UI = (uint)0x8232;
        public const uint GL_R16I = (uint)0x8233;
        public const uint GL_R16UI = (uint)0x8234;
        public const uint GL_R32I = (uint)0x8235;
        public const uint GL_R32UI = (uint)0x8236;
        public const uint GL_RG8I = (uint)0x8237;
        public const uint GL_RG8UI = (uint)0x8238;
        public const uint GL_RG16I = (uint)0x8239;
        public const uint GL_RG16UI = (uint)0x823A;
        public const uint GL_RG32I = (uint)0x823B;
        public const uint GL_RG32UI = (uint)0x823C;
        public const uint GL_VERTEX_ARRAY_BINDING = (uint)0x85B5;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glColorMaski(uint index, bool r, bool g, bool b, bool a);
        internal static glColorMaski _glColorMaski;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glColorMaski", IsExtension=false)]
        public static void ColorMaski(uint index, bool r, bool g, bool b, bool a)
        {
            _glColorMaski(index, r, g, b, a);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetBooleani_v(uint target, uint index, bool[] data);
        internal static glGetBooleani_v _glGetBooleani_v;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetBooleani_v", IsExtension=false)]
        public static void GetBooleani_v(uint target, uint index, bool[] data)
        {
            _glGetBooleani_v(target, index, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEnablei(uint target, uint index);
        internal static glEnablei _glEnablei;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEnablei", IsExtension=false)]
        public static void Enablei(uint target, uint index)
        {
            _glEnablei(target, index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDisablei(uint target, uint index);
        internal static glDisablei _glDisablei;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDisablei", IsExtension=false)]
        public static void Disablei(uint target, uint index)
        {
            _glDisablei(target, index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsEnabledi(uint target, uint index);
        internal static glIsEnabledi _glIsEnabledi;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsEnabledi", IsExtension=false)]
        public static bool IsEnabledi(uint target, uint index)
        {
            bool data = _glIsEnabledi(target, index);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBeginTransformFeedback(uint primitiveMode);
        internal static glBeginTransformFeedback _glBeginTransformFeedback;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBeginTransformFeedback", IsExtension=false)]
        public static void BeginTransformFeedback(uint primitiveMode)
        {
            _glBeginTransformFeedback(primitiveMode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEndTransformFeedback();
        internal static glEndTransformFeedback _glEndTransformFeedback;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEndTransformFeedback", IsExtension=false)]
        public static void EndTransformFeedback()
        {
            _glEndTransformFeedback();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTransformFeedbackVaryings(uint program, int count, string[] varyings, uint bufferMode);
        internal static glTransformFeedbackVaryings _glTransformFeedbackVaryings;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTransformFeedbackVaryings", IsExtension=false)]
        public static void TransformFeedbackVaryings(uint program, int count, string[] varyings, uint bufferMode)
        {
            _glTransformFeedbackVaryings(program, count, varyings, bufferMode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTransformFeedbackVarying(uint program, uint index, int bufSize, IntPtr length, IntPtr size, IntPtr type, byte[] name);
        internal static glGetTransformFeedbackVarying _glGetTransformFeedbackVarying;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTransformFeedbackVarying", IsExtension=false)]
        public static void GetTransformFeedbackVarying(uint program, uint index, int bufSize, IntPtr length, IntPtr size, IntPtr type, byte[] name)
        {
            _glGetTransformFeedbackVarying(program, index, bufSize, length, size, type, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClampColor(uint target, uint clamp);
        internal static glClampColor _glClampColor;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClampColor", IsExtension=false)]
        public static void ClampColor(uint target, uint clamp)
        {
            _glClampColor(target, clamp);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBeginConditionalRender(uint id, uint mode);
        internal static glBeginConditionalRender _glBeginConditionalRender;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBeginConditionalRender", IsExtension=false)]
        public static void BeginConditionalRender(uint id, uint mode)
        {
            _glBeginConditionalRender(id, mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEndConditionalRender();
        internal static glEndConditionalRender _glEndConditionalRender;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEndConditionalRender", IsExtension=false)]
        public static void EndConditionalRender()
        {
            _glEndConditionalRender();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribIPointer(uint index, int size, uint type, int stride, IntPtr pointer);
        internal static glVertexAttribIPointer _glVertexAttribIPointer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribIPointer", IsExtension=false)]
        public static void VertexAttribIPointer(uint index, int size, uint type, int stride, IntPtr pointer)
        {
            _glVertexAttribIPointer(index, size, type, stride, pointer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribIiv(uint index, uint pname, IntPtr @params);
        internal static glGetVertexAttribIiv _glGetVertexAttribIiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribIiv", IsExtension=false)]
        public static void GetVertexAttribIiv(uint index, uint pname, IntPtr @params)
        {
            _glGetVertexAttribIiv(index, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribIuiv(uint index, uint pname, IntPtr @params);
        internal static glGetVertexAttribIuiv _glGetVertexAttribIuiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribIuiv", IsExtension=false)]
        public static void GetVertexAttribIuiv(uint index, uint pname, IntPtr @params)
        {
            _glGetVertexAttribIuiv(index, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI1i(uint index, int x);
        internal static glVertexAttribI1i _glVertexAttribI1i;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI1i", IsExtension=false)]
        public static void VertexAttribI1i(uint index, int x)
        {
            _glVertexAttribI1i(index, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI2i(uint index, int x, int y);
        internal static glVertexAttribI2i _glVertexAttribI2i;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI2i", IsExtension=false)]
        public static void VertexAttribI2i(uint index, int x, int y)
        {
            _glVertexAttribI2i(index, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI3i(uint index, int x, int y, int z);
        internal static glVertexAttribI3i _glVertexAttribI3i;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI3i", IsExtension=false)]
        public static void VertexAttribI3i(uint index, int x, int y, int z)
        {
            _glVertexAttribI3i(index, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4i(uint index, int x, int y, int z, int w);
        internal static glVertexAttribI4i _glVertexAttribI4i;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4i", IsExtension=false)]
        public static void VertexAttribI4i(uint index, int x, int y, int z, int w)
        {
            _glVertexAttribI4i(index, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI1ui(uint index, uint x);
        internal static glVertexAttribI1ui _glVertexAttribI1ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI1ui", IsExtension=false)]
        public static void VertexAttribI1ui(uint index, uint x)
        {
            _glVertexAttribI1ui(index, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI2ui(uint index, uint x, uint y);
        internal static glVertexAttribI2ui _glVertexAttribI2ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI2ui", IsExtension=false)]
        public static void VertexAttribI2ui(uint index, uint x, uint y)
        {
            _glVertexAttribI2ui(index, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI3ui(uint index, uint x, uint y, uint z);
        internal static glVertexAttribI3ui _glVertexAttribI3ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI3ui", IsExtension=false)]
        public static void VertexAttribI3ui(uint index, uint x, uint y, uint z)
        {
            _glVertexAttribI3ui(index, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4ui(uint index, uint x, uint y, uint z, uint w);
        internal static glVertexAttribI4ui _glVertexAttribI4ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4ui", IsExtension=false)]
        public static void VertexAttribI4ui(uint index, uint x, uint y, uint z, uint w)
        {
            _glVertexAttribI4ui(index, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI1iv(uint index, IntPtr v);
        internal static glVertexAttribI1iv _glVertexAttribI1iv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI1iv", IsExtension=false)]
        public static void VertexAttribI1iv(uint index, IntPtr v)
        {
            _glVertexAttribI1iv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI2iv(uint index, int[] v);
        internal static glVertexAttribI2iv _glVertexAttribI2iv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI2iv", IsExtension=false)]
        public static void VertexAttribI2iv(uint index, int[] v)
        {
            _glVertexAttribI2iv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI3iv(uint index, int[] v);
        internal static glVertexAttribI3iv _glVertexAttribI3iv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI3iv", IsExtension=false)]
        public static void VertexAttribI3iv(uint index, int[] v)
        {
            _glVertexAttribI3iv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4iv(uint index, int[] v);
        internal static glVertexAttribI4iv _glVertexAttribI4iv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4iv", IsExtension=false)]
        public static void VertexAttribI4iv(uint index, int[] v)
        {
            _glVertexAttribI4iv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI1uiv(uint index, IntPtr v);
        internal static glVertexAttribI1uiv _glVertexAttribI1uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI1uiv", IsExtension=false)]
        public static void VertexAttribI1uiv(uint index, IntPtr v)
        {
            _glVertexAttribI1uiv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI2uiv(uint index, uint[] v);
        internal static glVertexAttribI2uiv _glVertexAttribI2uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI2uiv", IsExtension=false)]
        public static void VertexAttribI2uiv(uint index, uint[] v)
        {
            _glVertexAttribI2uiv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI3uiv(uint index, uint[] v);
        internal static glVertexAttribI3uiv _glVertexAttribI3uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI3uiv", IsExtension=false)]
        public static void VertexAttribI3uiv(uint index, uint[] v)
        {
            _glVertexAttribI3uiv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4uiv(uint index, uint[] v);
        internal static glVertexAttribI4uiv _glVertexAttribI4uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4uiv", IsExtension=false)]
        public static void VertexAttribI4uiv(uint index, uint[] v)
        {
            _glVertexAttribI4uiv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4bv(uint index, sbyte[] v);
        internal static glVertexAttribI4bv _glVertexAttribI4bv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4bv", IsExtension=false)]
        public static void VertexAttribI4bv(uint index, sbyte[] v)
        {
            _glVertexAttribI4bv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4sv(uint index, short[] v);
        internal static glVertexAttribI4sv _glVertexAttribI4sv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4sv", IsExtension=false)]
        public static void VertexAttribI4sv(uint index, short[] v)
        {
            _glVertexAttribI4sv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4ubv(uint index, string v);
        internal static glVertexAttribI4ubv _glVertexAttribI4ubv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4ubv", IsExtension=false)]
        public static void VertexAttribI4ubv(uint index, string v)
        {
            _glVertexAttribI4ubv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribI4usv(uint index, ushort[] v);
        internal static glVertexAttribI4usv _glVertexAttribI4usv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribI4usv", IsExtension=false)]
        public static void VertexAttribI4usv(uint index, ushort[] v)
        {
            _glVertexAttribI4usv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetUniformuiv(uint program, int location, uint[] @params);
        internal static glGetUniformuiv _glGetUniformuiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformuiv", IsExtension=false)]
        public static void GetUniformuiv(uint program, int location, uint[] @params)
        {
            _glGetUniformuiv(program, location, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindFragDataLocation(uint program, uint color, string name);
        internal static glBindFragDataLocation _glBindFragDataLocation;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindFragDataLocation", IsExtension=false)]
        public static void BindFragDataLocation(uint program, uint color, string name)
        {
            _glBindFragDataLocation(program, color, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate int glGetFragDataLocation(uint program, string name);
        internal static glGetFragDataLocation _glGetFragDataLocation;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetFragDataLocation", IsExtension=false)]
        public static int GetFragDataLocation(uint program, string name)
        {
            int data = _glGetFragDataLocation(program, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1ui(int location, uint v0);
        internal static glUniform1ui _glUniform1ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1ui", IsExtension=false)]
        public static void Uniform1ui(int location, uint v0)
        {
            _glUniform1ui(location, v0);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2ui(int location, uint v0, uint v1);
        internal static glUniform2ui _glUniform2ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2ui", IsExtension=false)]
        public static void Uniform2ui(int location, uint v0, uint v1)
        {
            _glUniform2ui(location, v0, v1);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3ui(int location, uint v0, uint v1, uint v2);
        internal static glUniform3ui _glUniform3ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3ui", IsExtension=false)]
        public static void Uniform3ui(int location, uint v0, uint v1, uint v2)
        {
            _glUniform3ui(location, v0, v1, v2);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3);
        internal static glUniform4ui _glUniform4ui;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4ui", IsExtension=false)]
        public static void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3)
        {
            _glUniform4ui(location, v0, v1, v2, v3);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1uiv(int location, int count, uint[] value);
        internal static glUniform1uiv _glUniform1uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1uiv", IsExtension=false)]
        public static void Uniform1uiv(int location, int count, uint[] value)
        {
            _glUniform1uiv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2uiv(int location, int count, uint[] value);
        internal static glUniform2uiv _glUniform2uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2uiv", IsExtension=false)]
        public static void Uniform2uiv(int location, int count, uint[] value)
        {
            _glUniform2uiv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3uiv(int location, int count, uint[] value);
        internal static glUniform3uiv _glUniform3uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3uiv", IsExtension=false)]
        public static void Uniform3uiv(int location, int count, uint[] value)
        {
            _glUniform3uiv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4uiv(int location, int count, uint[] value);
        internal static glUniform4uiv _glUniform4uiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4uiv", IsExtension=false)]
        public static void Uniform4uiv(int location, int count, uint[] value)
        {
            _glUniform4uiv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexParameterIiv(uint target, uint pname, int[] @params);
        internal static glTexParameterIiv _glTexParameterIiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexParameterIiv", IsExtension=false)]
        public static void TexParameterIiv(uint target, uint pname, int[] @params)
        {
            _glTexParameterIiv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexParameterIuiv(uint target, uint pname, uint[] @params);
        internal static glTexParameterIuiv _glTexParameterIuiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexParameterIuiv", IsExtension=false)]
        public static void TexParameterIuiv(uint target, uint pname, uint[] @params)
        {
            _glTexParameterIuiv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTexParameterIiv(uint target, uint pname, int[] @params);
        internal static glGetTexParameterIiv _glGetTexParameterIiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTexParameterIiv", IsExtension=false)]
        public static void GetTexParameterIiv(uint target, uint pname, int[] @params)
        {
            _glGetTexParameterIiv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTexParameterIuiv(uint target, uint pname, uint[] @params);
        internal static glGetTexParameterIuiv _glGetTexParameterIuiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTexParameterIuiv", IsExtension=false)]
        public static void GetTexParameterIuiv(uint target, uint pname, uint[] @params)
        {
            _glGetTexParameterIuiv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearBufferiv(uint buffer, int drawbuffer, int[] value);
        internal static glClearBufferiv _glClearBufferiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearBufferiv", IsExtension=false)]
        public static void ClearBufferiv(uint buffer, int drawbuffer, int[] value)
        {
            _glClearBufferiv(buffer, drawbuffer, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearBufferuiv(uint buffer, int drawbuffer, uint[] value);
        internal static glClearBufferuiv _glClearBufferuiv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearBufferuiv", IsExtension=false)]
        public static void ClearBufferuiv(uint buffer, int drawbuffer, uint[] value)
        {
            _glClearBufferuiv(buffer, drawbuffer, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearBufferfv(uint buffer, int drawbuffer, float[] value);
        internal static glClearBufferfv _glClearBufferfv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearBufferfv", IsExtension=false)]
        public static void ClearBufferfv(uint buffer, int drawbuffer, float[] value)
        {
            _glClearBufferfv(buffer, drawbuffer, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil);
        internal static glClearBufferfi _glClearBufferfi;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearBufferfi", IsExtension=false)]
        public static void ClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil)
        {
            _glClearBufferfi(buffer, drawbuffer, depth, stencil);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glGetStringi(uint name, uint index);
        internal static glGetStringi _glGetStringi;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetStringi", IsExtension=false)]
        public static string GetStringi(uint name, uint index)
        {
            IntPtr data = _glGetStringi(name, index);
            return Marshal.PtrToStringAnsi(data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsRenderbuffer(uint renderbuffer);
        internal static glIsRenderbuffer _glIsRenderbuffer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsRenderbuffer", IsExtension=false)]
        public static bool IsRenderbuffer(uint renderbuffer)
        {
            bool data = _glIsRenderbuffer(renderbuffer);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindRenderbuffer(uint target, uint renderbuffer);
        internal static glBindRenderbuffer _glBindRenderbuffer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindRenderbuffer", IsExtension=false)]
        public static void BindRenderbuffer(uint target, uint renderbuffer)
        {
            _glBindRenderbuffer(target, renderbuffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteRenderbuffers(int n, uint[] renderbuffers);
        internal static glDeleteRenderbuffers _glDeleteRenderbuffers;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteRenderbuffers", IsExtension=false)]
        public static void DeleteRenderbuffers(int n, uint[] renderbuffers)
        {
            _glDeleteRenderbuffers(n, renderbuffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenRenderbuffers(int n, uint[] renderbuffers);
        internal static glGenRenderbuffers _glGenRenderbuffers;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenRenderbuffers", IsExtension=false)]
        public static void GenRenderbuffers(int n, uint[] renderbuffers)
        {
            _glGenRenderbuffers(n, renderbuffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glRenderbufferStorage(uint target, uint internalformat, int width, int height);
        internal static glRenderbufferStorage _glRenderbufferStorage;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glRenderbufferStorage", IsExtension=false)]
        public static void RenderbufferStorage(uint target, uint internalformat, int width, int height)
        {
            _glRenderbufferStorage(target, internalformat, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetRenderbufferParameteriv(uint target, uint pname, int[] @params);
        internal static glGetRenderbufferParameteriv _glGetRenderbufferParameteriv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetRenderbufferParameteriv", IsExtension=false)]
        public static void GetRenderbufferParameteriv(uint target, uint pname, int[] @params)
        {
            _glGetRenderbufferParameteriv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsFramebuffer(uint framebuffer);
        internal static glIsFramebuffer _glIsFramebuffer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsFramebuffer", IsExtension=false)]
        public static bool IsFramebuffer(uint framebuffer)
        {
            bool data = _glIsFramebuffer(framebuffer);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindFramebuffer(uint target, uint framebuffer);
        internal static glBindFramebuffer _glBindFramebuffer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindFramebuffer", IsExtension=false)]
        public static void BindFramebuffer(uint target, uint framebuffer)
        {
            _glBindFramebuffer(target, framebuffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteFramebuffers(int n, uint[] framebuffers);
        internal static glDeleteFramebuffers _glDeleteFramebuffers;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteFramebuffers", IsExtension=false)]
        public static void DeleteFramebuffers(int n, uint[] framebuffers)
        {
            _glDeleteFramebuffers(n, framebuffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenFramebuffers(int n, uint[] framebuffers);
        internal static glGenFramebuffers _glGenFramebuffers;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenFramebuffers", IsExtension=false)]
        public static void GenFramebuffers(int n, uint[] framebuffers)
        {
            _glGenFramebuffers(n, framebuffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glCheckFramebufferStatus(uint target);
        internal static glCheckFramebufferStatus _glCheckFramebufferStatus;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCheckFramebufferStatus", IsExtension=false)]
        public static uint CheckFramebufferStatus(uint target)
        {
            uint data = _glCheckFramebufferStatus(target);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level);
        internal static glFramebufferTexture1D _glFramebufferTexture1D;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferTexture1D", IsExtension=false)]
        public static void FramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level)
        {
            _glFramebufferTexture1D(target, attachment, textarget, texture, level);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level);
        internal static glFramebufferTexture2D _glFramebufferTexture2D;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferTexture2D", IsExtension=false)]
        public static void FramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level)
        {
            _glFramebufferTexture2D(target, attachment, textarget, texture, level);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset);
        internal static glFramebufferTexture3D _glFramebufferTexture3D;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferTexture3D", IsExtension=false)]
        public static void FramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset)
        {
            _glFramebufferTexture3D(target, attachment, textarget, texture, level, zoffset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
        internal static glFramebufferRenderbuffer _glFramebufferRenderbuffer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferRenderbuffer", IsExtension=false)]
        public static void FramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
        {
            _glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int[] @params);
        internal static glGetFramebufferAttachmentParameteriv _glGetFramebufferAttachmentParameteriv;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetFramebufferAttachmentParameteriv", IsExtension=false)]
        public static void GetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int[] @params)
        {
            _glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenerateMipmap(uint target);
        internal static glGenerateMipmap _glGenerateMipmap;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenerateMipmap", IsExtension=false)]
        public static void GenerateMipmap(uint target)
        {
            _glGenerateMipmap(target);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
        internal static glBlitFramebuffer _glBlitFramebuffer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlitFramebuffer", IsExtension=false)]
        public static void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter)
        {
            _glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height);
        internal static glRenderbufferStorageMultisample _glRenderbufferStorageMultisample;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glRenderbufferStorageMultisample", IsExtension=false)]
        public static void RenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height)
        {
            _glRenderbufferStorageMultisample(target, samples, internalformat, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer);
        internal static glFramebufferTextureLayer _glFramebufferTextureLayer;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferTextureLayer", IsExtension=false)]
        public static void FramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer)
        {
            _glFramebufferTextureLayer(target, attachment, texture, level, layer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glMapBufferRange(uint target, IntPtr offset, IntPtr length, uint access);
        internal static glMapBufferRange _glMapBufferRange;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMapBufferRange", IsExtension=false)]
        public static IntPtr MapBufferRange(uint target, IntPtr offset, IntPtr length, uint access)
        {
            IntPtr data = _glMapBufferRange(target, offset, length, access);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFlushMappedBufferRange(uint target, IntPtr offset, IntPtr length);
        internal static glFlushMappedBufferRange _glFlushMappedBufferRange;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFlushMappedBufferRange", IsExtension=false)]
        public static void FlushMappedBufferRange(uint target, IntPtr offset, IntPtr length)
        {
            _glFlushMappedBufferRange(target, offset, length);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindVertexArray(uint array);
        internal static glBindVertexArray _glBindVertexArray;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindVertexArray", IsExtension=false)]
        public static void BindVertexArray(uint array)
        {
            _glBindVertexArray(array);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteVertexArrays(int n, uint[] arrays);
        internal static glDeleteVertexArrays _glDeleteVertexArrays;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteVertexArrays", IsExtension=false)]
        public static void DeleteVertexArrays(int n, uint[] arrays)
        {
            _glDeleteVertexArrays(n, arrays);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenVertexArrays(int n, uint[] arrays);
        internal static glGenVertexArrays _glGenVertexArrays;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenVertexArrays", IsExtension=false)]
        public static void GenVertexArrays(int n, uint[] arrays)
        {
            _glGenVertexArrays(n, arrays);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsVertexArray(uint array);
        internal static glIsVertexArray _glIsVertexArray;

        [Version(Group="GL_VERSION_3_0", Version = "3.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsVertexArray", IsExtension=false)]
        public static bool IsVertexArray(uint array)
        {
            bool data = _glIsVertexArray(array);
            return data;
        }
        

        #endregion

        #region GL_VERSION_3_1
        public const uint GL_SAMPLER_2D_RECT = (uint)0x8B63;
        public const uint GL_SAMPLER_2D_RECT_SHADOW = (uint)0x8B64;
        public const uint GL_SAMPLER_BUFFER = (uint)0x8DC2;
        public const uint GL_INT_SAMPLER_2D_RECT = (uint)0x8DCD;
        public const uint GL_INT_SAMPLER_BUFFER = (uint)0x8DD0;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D_RECT = (uint)0x8DD5;
        public const uint GL_UNSIGNED_INT_SAMPLER_BUFFER = (uint)0x8DD8;
        public const uint GL_TEXTURE_BUFFER = (uint)0x8C2A;
        public const uint GL_MAX_TEXTURE_BUFFER_SIZE = (uint)0x8C2B;
        public const uint GL_TEXTURE_BUFFER_DATA_STORE_BINDING = (uint)0x8C2D;
        public const uint GL_TEXTURE_RECTANGLE = (uint)0x84F5;
        public const uint GL_PROXY_TEXTURE_RECTANGLE = (uint)0x84F7;
        public const uint GL_MAX_RECTANGLE_TEXTURE_SIZE = (uint)0x84F8;
        public const uint GL_R8_SNORM = (uint)0x8F94;
        public const uint GL_RG8_SNORM = (uint)0x8F95;
        public const uint GL_RGB8_SNORM = (uint)0x8F96;
        public const uint GL_RGBA8_SNORM = (uint)0x8F97;
        public const uint GL_R16_SNORM = (uint)0x8F98;
        public const uint GL_RG16_SNORM = (uint)0x8F99;
        public const uint GL_RGB16_SNORM = (uint)0x8F9A;
        public const uint GL_RGBA16_SNORM = (uint)0x8F9B;
        public const uint GL_SIGNED_NORMALIZED = (uint)0x8F9C;
        public const uint GL_PRIMITIVE_RESTART = (uint)0x8F9D;
        public const uint GL_PRIMITIVE_RESTART_INDEX = (uint)0x8F9E;
        public const uint GL_COPY_READ_BUFFER = (uint)0x8F36;
        public const uint GL_COPY_WRITE_BUFFER = (uint)0x8F37;
        public const uint GL_UNIFORM_BUFFER = (uint)0x8A11;
        public const uint GL_UNIFORM_BUFFER_BINDING = (uint)0x8A28;
        public const uint GL_UNIFORM_BUFFER_START = (uint)0x8A29;
        public const uint GL_UNIFORM_BUFFER_SIZE = (uint)0x8A2A;
        public const uint GL_MAX_VERTEX_UNIFORM_BLOCKS = (uint)0x8A2B;
        public const uint GL_MAX_GEOMETRY_UNIFORM_BLOCKS = (uint)0x8A2C;
        public const uint GL_MAX_FRAGMENT_UNIFORM_BLOCKS = (uint)0x8A2D;
        public const uint GL_MAX_COMBINED_UNIFORM_BLOCKS = (uint)0x8A2E;
        public const uint GL_MAX_UNIFORM_BUFFER_BINDINGS = (uint)0x8A2F;
        public const uint GL_MAX_UNIFORM_BLOCK_SIZE = (uint)0x8A30;
        public const uint GL_MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS = (uint)0x8A31;
        public const uint GL_MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS = (uint)0x8A32;
        public const uint GL_MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS = (uint)0x8A33;
        public const uint GL_UNIFORM_BUFFER_OFFSET_ALIGNMENT = (uint)0x8A34;
        public const uint GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH = (uint)0x8A35;
        public const uint GL_ACTIVE_UNIFORM_BLOCKS = (uint)0x8A36;
        public const uint GL_UNIFORM_TYPE = (uint)0x8A37;
        public const uint GL_UNIFORM_SIZE = (uint)0x8A38;
        public const uint GL_UNIFORM_NAME_LENGTH = (uint)0x8A39;
        public const uint GL_UNIFORM_BLOCK_INDEX = (uint)0x8A3A;
        public const uint GL_UNIFORM_OFFSET = (uint)0x8A3B;
        public const uint GL_UNIFORM_ARRAY_STRIDE = (uint)0x8A3C;
        public const uint GL_UNIFORM_MATRIX_STRIDE = (uint)0x8A3D;
        public const uint GL_UNIFORM_IS_ROW_MAJOR = (uint)0x8A3E;
        public const uint GL_UNIFORM_BLOCK_BINDING = (uint)0x8A3F;
        public const uint GL_UNIFORM_BLOCK_DATA_SIZE = (uint)0x8A40;
        public const uint GL_UNIFORM_BLOCK_NAME_LENGTH = (uint)0x8A41;
        public const uint GL_UNIFORM_BLOCK_ACTIVE_UNIFORMS = (uint)0x8A42;
        public const uint GL_UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES = (uint)0x8A43;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER = (uint)0x8A44;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_GEOMETRY_SHADER = (uint)0x8A45;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER = (uint)0x8A46;
        public const uint GL_INVALID_INDEX = (uint)0xFFFFFFFF;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetIntegeri_v(uint target, uint index, int[] data);
        internal static glGetIntegeri_v _glGetIntegeri_v;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetIntegeri_v", IsExtension=false)]
        public static void GetIntegeri_v(uint target, uint index, int[] data)
        {
            _glGetIntegeri_v(target, index, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindBufferRange(uint target, uint index, uint buffer, IntPtr offset, IntPtr size);
        internal static glBindBufferRange _glBindBufferRange;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindBufferRange", IsExtension=false)]
        public static void BindBufferRange(uint target, uint index, uint buffer, IntPtr offset, IntPtr size)
        {
            _glBindBufferRange(target, index, buffer, offset, size);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindBufferBase(uint target, uint index, uint buffer);
        internal static glBindBufferBase _glBindBufferBase;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindBufferBase", IsExtension=false)]
        public static void BindBufferBase(uint target, uint index, uint buffer)
        {
            _glBindBufferBase(target, index, buffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawArraysInstanced(uint mode, int first, int count, int instancecount);
        internal static glDrawArraysInstanced _glDrawArraysInstanced;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawArraysInstanced", IsExtension=false)]
        public static void DrawArraysInstanced(uint mode, int first, int count, int instancecount)
        {
            _glDrawArraysInstanced(mode, first, count, instancecount);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElementsInstanced(uint mode, int count, uint type, IntPtr indices, int instancecount);
        internal static glDrawElementsInstanced _glDrawElementsInstanced;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElementsInstanced", IsExtension=false)]
        public static void DrawElementsInstanced(uint mode, int count, uint type, IntPtr indices, int instancecount)
        {
            _glDrawElementsInstanced(mode, count, type, indices, instancecount);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexBuffer(uint target, uint internalformat, uint buffer);
        internal static glTexBuffer _glTexBuffer;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexBuffer", IsExtension=false)]
        public static void TexBuffer(uint target, uint internalformat, uint buffer)
        {
            _glTexBuffer(target, internalformat, buffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPrimitiveRestartIndex(uint index);
        internal static glPrimitiveRestartIndex _glPrimitiveRestartIndex;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPrimitiveRestartIndex", IsExtension=false)]
        public static void PrimitiveRestartIndex(uint index)
        {
            _glPrimitiveRestartIndex(index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyBufferSubData(uint readTarget, uint writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size);
        internal static glCopyBufferSubData _glCopyBufferSubData;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyBufferSubData", IsExtension=false)]
        public static void CopyBufferSubData(uint readTarget, uint writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size)
        {
            _glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetUniformIndices(uint program, int uniformCount, string[] uniformNames, uint[] uniformIndices);
        internal static glGetUniformIndices _glGetUniformIndices;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformIndices", IsExtension=false)]
        public static void GetUniformIndices(uint program, int uniformCount, string[] uniformNames, uint[] uniformIndices)
        {
            _glGetUniformIndices(program, uniformCount, uniformNames, uniformIndices);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveUniformsiv(uint program, int uniformCount, uint[] uniformIndices, uint pname, int[] @params);
        internal static glGetActiveUniformsiv _glGetActiveUniformsiv;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveUniformsiv", IsExtension=false)]
        public static void GetActiveUniformsiv(uint program, int uniformCount, uint[] uniformIndices, uint pname, int[] @params)
        {
            _glGetActiveUniformsiv(program, uniformCount, uniformIndices, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveUniformName(uint program, uint uniformIndex, int bufSize, IntPtr length, byte[] uniformName);
        internal static glGetActiveUniformName _glGetActiveUniformName;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveUniformName", IsExtension=false)]
        public static void GetActiveUniformName(uint program, uint uniformIndex, int bufSize, IntPtr length, byte[] uniformName)
        {
            _glGetActiveUniformName(program, uniformIndex, bufSize, length, uniformName);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetUniformBlockIndex(uint program, string uniformBlockName);
        internal static glGetUniformBlockIndex _glGetUniformBlockIndex;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformBlockIndex", IsExtension=false)]
        public static uint GetUniformBlockIndex(uint program, string uniformBlockName)
        {
            uint data = _glGetUniformBlockIndex(program, uniformBlockName);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, int[] @params);
        internal static glGetActiveUniformBlockiv _glGetActiveUniformBlockiv;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveUniformBlockiv", IsExtension=false)]
        public static void GetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, int[] @params)
        {
            _glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, IntPtr length, byte[] uniformBlockName);
        internal static glGetActiveUniformBlockName _glGetActiveUniformBlockName;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveUniformBlockName", IsExtension=false)]
        public static void GetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, IntPtr length, byte[] uniformBlockName)
        {
            _glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, length, uniformBlockName);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding);
        internal static glUniformBlockBinding _glUniformBlockBinding;

        [Version(Group="GL_VERSION_3_1", Version = "3.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformBlockBinding", IsExtension=false)]
        public static void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding)
        {
            _glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
        }
        

        #endregion

        #region GL_VERSION_3_2
        public const uint GL_CONTEXT_CORE_PROFILE_BIT = (uint)0x00000001;
        public const uint GL_CONTEXT_COMPATIBILITY_PROFILE_BIT = (uint)0x00000002;
        public const uint GL_LINES_ADJACENCY = (uint)0x000A;
        public const uint GL_LINE_STRIP_ADJACENCY = (uint)0x000B;
        public const uint GL_TRIANGLES_ADJACENCY = (uint)0x000C;
        public const uint GL_TRIANGLE_STRIP_ADJACENCY = (uint)0x000D;
        public const uint GL_PROGRAM_POINT_SIZE = (uint)0x8642;
        public const uint GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS = (uint)0x8C29;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_LAYERED = (uint)0x8DA7;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS = (uint)0x8DA8;
        public const uint GL_GEOMETRY_SHADER = (uint)0x8DD9;
        public const uint GL_GEOMETRY_VERTICES_OUT = (uint)0x8916;
        public const uint GL_GEOMETRY_INPUT_TYPE = (uint)0x8917;
        public const uint GL_GEOMETRY_OUTPUT_TYPE = (uint)0x8918;
        public const uint GL_MAX_GEOMETRY_UNIFORM_COMPONENTS = (uint)0x8DDF;
        public const uint GL_MAX_GEOMETRY_OUTPUT_VERTICES = (uint)0x8DE0;
        public const uint GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS = (uint)0x8DE1;
        public const uint GL_MAX_VERTEX_OUTPUT_COMPONENTS = (uint)0x9122;
        public const uint GL_MAX_GEOMETRY_INPUT_COMPONENTS = (uint)0x9123;
        public const uint GL_MAX_GEOMETRY_OUTPUT_COMPONENTS = (uint)0x9124;
        public const uint GL_MAX_FRAGMENT_INPUT_COMPONENTS = (uint)0x9125;
        public const uint GL_CONTEXT_PROFILE_MASK = (uint)0x9126;
        public const uint GL_DEPTH_CLAMP = (uint)0x864F;
        public const uint GL_QUADS_FOLLOW_PROVOKING_VERTEX_CONVENTION = (uint)0x8E4C;
        public const uint GL_FIRST_VERTEX_CONVENTION = (uint)0x8E4D;
        public const uint GL_LAST_VERTEX_CONVENTION = (uint)0x8E4E;
        public const uint GL_PROVOKING_VERTEX = (uint)0x8E4F;
        public const uint GL_TEXTURE_CUBE_MAP_SEAMLESS = (uint)0x884F;
        public const uint GL_MAX_SERVER_WAIT_TIMEOUT = (uint)0x9111;
        public const uint GL_OBJECT_TYPE = (uint)0x9112;
        public const uint GL_SYNC_CONDITION = (uint)0x9113;
        public const uint GL_SYNC_STATUS = (uint)0x9114;
        public const uint GL_SYNC_FLAGS = (uint)0x9115;
        public const uint GL_SYNC_FENCE = (uint)0x9116;
        public const uint GL_SYNC_GPU_COMMANDS_COMPLETE = (uint)0x9117;
        public const uint GL_UNSIGNALED = (uint)0x9118;
        public const uint GL_SIGNALED = (uint)0x9119;
        public const uint GL_ALREADY_SIGNALED = (uint)0x911A;
        public const uint GL_TIMEOUT_EXPIRED = (uint)0x911B;
        public const uint GL_CONDITION_SATISFIED = (uint)0x911C;
        public const uint GL_WAIT_FAILED = (uint)0x911D;
        public const ulong GL_TIMEOUT_IGNORED = (ulong)0xFFFFFFFFFFFFFFFF;
        public const uint GL_SYNC_FLUSH_COMMANDS_BIT = (uint)0x00000001;
        public const uint GL_SAMPLE_POSITION = (uint)0x8E50;
        public const uint GL_SAMPLE_MASK = (uint)0x8E51;
        public const uint GL_SAMPLE_MASK_VALUE = (uint)0x8E52;
        public const uint GL_MAX_SAMPLE_MASK_WORDS = (uint)0x8E59;
        public const uint GL_TEXTURE_2D_MULTISAMPLE = (uint)0x9100;
        public const uint GL_PROXY_TEXTURE_2D_MULTISAMPLE = (uint)0x9101;
        public const uint GL_TEXTURE_2D_MULTISAMPLE_ARRAY = (uint)0x9102;
        public const uint GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY = (uint)0x9103;
        public const uint GL_TEXTURE_SAMPLES = (uint)0x9106;
        public const uint GL_TEXTURE_FIXED_SAMPLE_LOCATIONS = (uint)0x9107;
        public const uint GL_SAMPLER_2D_MULTISAMPLE = (uint)0x9108;
        public const uint GL_INT_SAMPLER_2D_MULTISAMPLE = (uint)0x9109;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE = (uint)0x910A;
        public const uint GL_SAMPLER_2D_MULTISAMPLE_ARRAY = (uint)0x910B;
        public const uint GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = (uint)0x910C;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = (uint)0x910D;
        public const uint GL_MAX_COLOR_TEXTURE_SAMPLES = (uint)0x910E;
        public const uint GL_MAX_DEPTH_TEXTURE_SAMPLES = (uint)0x910F;
        public const uint GL_MAX_INTEGER_SAMPLES = (uint)0x9110;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElementsBaseVertex(uint mode, int count, uint type, IntPtr indices, int basevertex);
        internal static glDrawElementsBaseVertex _glDrawElementsBaseVertex;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElementsBaseVertex", IsExtension=false)]
        public static void DrawElementsBaseVertex(uint mode, int count, uint type, IntPtr indices, int basevertex)
        {
            _glDrawElementsBaseVertex(mode, count, type, indices, basevertex);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, IntPtr indices, int basevertex);
        internal static glDrawRangeElementsBaseVertex _glDrawRangeElementsBaseVertex;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawRangeElementsBaseVertex", IsExtension=false)]
        public static void DrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, IntPtr indices, int basevertex)
        {
            _glDrawRangeElementsBaseVertex(mode, start, end, count, type, indices, basevertex);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex);
        internal static glDrawElementsInstancedBaseVertex _glDrawElementsInstancedBaseVertex;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElementsInstancedBaseVertex", IsExtension=false)]
        public static void DrawElementsInstancedBaseVertex(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex)
        {
            _glDrawElementsInstancedBaseVertex(mode, count, type, indices, instancecount, basevertex);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMultiDrawElementsBaseVertex(uint mode, int[] count, uint type, IntPtr indices, int drawcount, int[] basevertex);
        internal static glMultiDrawElementsBaseVertex _glMultiDrawElementsBaseVertex;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMultiDrawElementsBaseVertex", IsExtension=false)]
        public static void MultiDrawElementsBaseVertex(uint mode, int[] count, uint type, IntPtr indices, int drawcount, int[] basevertex)
        {
            _glMultiDrawElementsBaseVertex(mode, count, type, indices, drawcount, basevertex);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProvokingVertex(uint mode);
        internal static glProvokingVertex _glProvokingVertex;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProvokingVertex", IsExtension=false)]
        public static void ProvokingVertex(uint mode)
        {
            _glProvokingVertex(mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glFenceSync(uint condition, uint flags);
        internal static glFenceSync _glFenceSync;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFenceSync", IsExtension=false)]
        public static IntPtr FenceSync(uint condition, uint flags)
        {
            IntPtr data = _glFenceSync(condition, flags);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsSync(IntPtr sync);
        internal static glIsSync _glIsSync;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsSync", IsExtension=false)]
        public static bool IsSync(IntPtr sync)
        {
            bool data = _glIsSync(sync);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteSync(IntPtr sync);
        internal static glDeleteSync _glDeleteSync;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteSync", IsExtension=false)]
        public static void DeleteSync(IntPtr sync)
        {
            _glDeleteSync(sync);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glClientWaitSync(IntPtr sync, uint flags, ulong timeout);
        internal static glClientWaitSync _glClientWaitSync;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClientWaitSync", IsExtension=false)]
        public static uint ClientWaitSync(IntPtr sync, uint flags, ulong timeout)
        {
            uint data = _glClientWaitSync(sync, flags, timeout);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glWaitSync(IntPtr sync, uint flags, ulong timeout);
        internal static glWaitSync _glWaitSync;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glWaitSync", IsExtension=false)]
        public static void WaitSync(IntPtr sync, uint flags, ulong timeout)
        {
            _glWaitSync(sync, flags, timeout);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetInteger64v(uint pname, long[] data);
        internal static glGetInteger64v _glGetInteger64v;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetInteger64v", IsExtension=false)]
        public static void GetInteger64v(uint pname, long[] data)
        {
            _glGetInteger64v(pname, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetSynciv(IntPtr sync, uint pname, int bufSize, IntPtr length, int[] values);
        internal static glGetSynciv _glGetSynciv;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetSynciv", IsExtension=false)]
        public static void GetSynciv(IntPtr sync, uint pname, int bufSize, IntPtr length, int[] values)
        {
            _glGetSynciv(sync, pname, bufSize, length, values);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetInteger64i_v(uint target, uint index, long[] data);
        internal static glGetInteger64i_v _glGetInteger64i_v;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetInteger64i_v", IsExtension=false)]
        public static void GetInteger64i_v(uint target, uint index, long[] data)
        {
            _glGetInteger64i_v(target, index, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetBufferParameteri64v(uint target, uint pname, long[] @params);
        internal static glGetBufferParameteri64v _glGetBufferParameteri64v;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetBufferParameteri64v", IsExtension=false)]
        public static void GetBufferParameteri64v(uint target, uint pname, long[] @params)
        {
            _glGetBufferParameteri64v(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferTexture(uint target, uint attachment, uint texture, int level);
        internal static glFramebufferTexture _glFramebufferTexture;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferTexture", IsExtension=false)]
        public static void FramebufferTexture(uint target, uint attachment, uint texture, int level)
        {
            _glFramebufferTexture(target, attachment, texture, level);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexImage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
        internal static glTexImage2DMultisample _glTexImage2DMultisample;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexImage2DMultisample", IsExtension=false)]
        public static void TexImage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations)
        {
            _glTexImage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexImage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
        internal static glTexImage3DMultisample _glTexImage3DMultisample;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexImage3DMultisample", IsExtension=false)]
        public static void TexImage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations)
        {
            _glTexImage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetMultisamplefv(uint pname, uint index, float[] val);
        internal static glGetMultisamplefv _glGetMultisamplefv;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetMultisamplefv", IsExtension=false)]
        public static void GetMultisamplefv(uint pname, uint index, float[] val)
        {
            _glGetMultisamplefv(pname, index, val);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSampleMaski(uint maskNumber, uint mask);
        internal static glSampleMaski _glSampleMaski;

        [Version(Group="GL_VERSION_3_2", Version = "3.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSampleMaski", IsExtension=false)]
        public static void SampleMaski(uint maskNumber, uint mask)
        {
            _glSampleMaski(maskNumber, mask);
        }
        

        #endregion

        #region GL_VERSION_3_3
        public const uint GL_VERTEX_ATTRIB_ARRAY_DIVISOR = (uint)0x88FE;
        public const uint GL_SRC1_COLOR = (uint)0x88F9;
        public const uint GL_ONE_MINUS_SRC1_COLOR = (uint)0x88FA;
        public const uint GL_ONE_MINUS_SRC1_ALPHA = (uint)0x88FB;
        public const uint GL_MAX_DUAL_SOURCE_DRAW_BUFFERS = (uint)0x88FC;
        public const uint GL_ANY_SAMPLES_PASSED = (uint)0x8C2F;
        public const uint GL_SAMPLER_BINDING = (uint)0x8919;
        public const uint GL_RGB10_A2UI = (uint)0x906F;
        public const uint GL_TEXTURE_SWIZZLE_R = (uint)0x8E42;
        public const uint GL_TEXTURE_SWIZZLE_G = (uint)0x8E43;
        public const uint GL_TEXTURE_SWIZZLE_B = (uint)0x8E44;
        public const uint GL_TEXTURE_SWIZZLE_A = (uint)0x8E45;
        public const uint GL_TEXTURE_SWIZZLE_RGBA = (uint)0x8E46;
        public const uint GL_TIME_ELAPSED = (uint)0x88BF;
        public const uint GL_TIMESTAMP = (uint)0x8E28;
        public const uint GL_INT_2_10_10_10_REV = (uint)0x8D9F;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindFragDataLocationIndexed(uint program, uint colorNumber, uint index, IntPtr name);
        internal static glBindFragDataLocationIndexed _glBindFragDataLocationIndexed;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindFragDataLocationIndexed", IsExtension=false)]
        public static void BindFragDataLocationIndexed(uint program, uint colorNumber, uint index, IntPtr name)
        {
            _glBindFragDataLocationIndexed(program, colorNumber, index, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate int glGetFragDataIndex(uint program, IntPtr name);
        internal static glGetFragDataIndex _glGetFragDataIndex;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetFragDataIndex", IsExtension=false)]
        public static int GetFragDataIndex(uint program, IntPtr name)
        {
            int data = _glGetFragDataIndex(program, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenSamplers(int count, uint[] samplers);
        internal static glGenSamplers _glGenSamplers;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenSamplers", IsExtension=false)]
        public static void GenSamplers(int count, uint[] samplers)
        {
            _glGenSamplers(count, samplers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteSamplers(int count, uint[] samplers);
        internal static glDeleteSamplers _glDeleteSamplers;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteSamplers", IsExtension=false)]
        public static void DeleteSamplers(int count, uint[] samplers)
        {
            _glDeleteSamplers(count, samplers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsSampler(uint sampler);
        internal static glIsSampler _glIsSampler;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsSampler", IsExtension=false)]
        public static bool IsSampler(uint sampler)
        {
            bool data = _glIsSampler(sampler);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindSampler(uint unit, uint sampler);
        internal static glBindSampler _glBindSampler;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindSampler", IsExtension=false)]
        public static void BindSampler(uint unit, uint sampler)
        {
            _glBindSampler(unit, sampler);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSamplerParameteri(uint sampler, uint pname, int param);
        internal static glSamplerParameteri _glSamplerParameteri;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSamplerParameteri", IsExtension=false)]
        public static void SamplerParameteri(uint sampler, uint pname, int param)
        {
            _glSamplerParameteri(sampler, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSamplerParameteriv(uint sampler, uint pname, int[] param);
        internal static glSamplerParameteriv _glSamplerParameteriv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSamplerParameteriv", IsExtension=false)]
        public static void SamplerParameteriv(uint sampler, uint pname, int[] param)
        {
            _glSamplerParameteriv(sampler, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSamplerParameterf(uint sampler, uint pname, float param);
        internal static glSamplerParameterf _glSamplerParameterf;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSamplerParameterf", IsExtension=false)]
        public static void SamplerParameterf(uint sampler, uint pname, float param)
        {
            _glSamplerParameterf(sampler, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSamplerParameterfv(uint sampler, uint pname, float[] param);
        internal static glSamplerParameterfv _glSamplerParameterfv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSamplerParameterfv", IsExtension=false)]
        public static void SamplerParameterfv(uint sampler, uint pname, float[] param)
        {
            _glSamplerParameterfv(sampler, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSamplerParameterIiv(uint sampler, uint pname, int[] param);
        internal static glSamplerParameterIiv _glSamplerParameterIiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSamplerParameterIiv", IsExtension=false)]
        public static void SamplerParameterIiv(uint sampler, uint pname, int[] param)
        {
            _glSamplerParameterIiv(sampler, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSamplerParameterIuiv(uint sampler, uint pname, uint[] param);
        internal static glSamplerParameterIuiv _glSamplerParameterIuiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSamplerParameterIuiv", IsExtension=false)]
        public static void SamplerParameterIuiv(uint sampler, uint pname, uint[] param)
        {
            _glSamplerParameterIuiv(sampler, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetSamplerParameteriv(uint sampler, uint pname, int[] @params);
        internal static glGetSamplerParameteriv _glGetSamplerParameteriv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetSamplerParameteriv", IsExtension=false)]
        public static void GetSamplerParameteriv(uint sampler, uint pname, int[] @params)
        {
            _glGetSamplerParameteriv(sampler, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetSamplerParameterIiv(uint sampler, uint pname, int[] @params);
        internal static glGetSamplerParameterIiv _glGetSamplerParameterIiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetSamplerParameterIiv", IsExtension=false)]
        public static void GetSamplerParameterIiv(uint sampler, uint pname, int[] @params)
        {
            _glGetSamplerParameterIiv(sampler, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetSamplerParameterfv(uint sampler, uint pname, float[] @params);
        internal static glGetSamplerParameterfv _glGetSamplerParameterfv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetSamplerParameterfv", IsExtension=false)]
        public static void GetSamplerParameterfv(uint sampler, uint pname, float[] @params)
        {
            _glGetSamplerParameterfv(sampler, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetSamplerParameterIuiv(uint sampler, uint pname, uint[] @params);
        internal static glGetSamplerParameterIuiv _glGetSamplerParameterIuiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetSamplerParameterIuiv", IsExtension=false)]
        public static void GetSamplerParameterIuiv(uint sampler, uint pname, uint[] @params)
        {
            _glGetSamplerParameterIuiv(sampler, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glQueryCounter(uint id, uint target);
        internal static glQueryCounter _glQueryCounter;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glQueryCounter", IsExtension=false)]
        public static void QueryCounter(uint id, uint target)
        {
            _glQueryCounter(id, target);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryObjecti64v(uint id, uint pname, long[] @params);
        internal static glGetQueryObjecti64v _glGetQueryObjecti64v;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryObjecti64v", IsExtension=false)]
        public static void GetQueryObjecti64v(uint id, uint pname, long[] @params)
        {
            _glGetQueryObjecti64v(id, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryObjectui64v(uint id, uint pname, ulong[] @params);
        internal static glGetQueryObjectui64v _glGetQueryObjectui64v;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryObjectui64v", IsExtension=false)]
        public static void GetQueryObjectui64v(uint id, uint pname, ulong[] @params)
        {
            _glGetQueryObjectui64v(id, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribDivisor(uint index, uint divisor);
        internal static glVertexAttribDivisor _glVertexAttribDivisor;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribDivisor", IsExtension=false)]
        public static void VertexAttribDivisor(uint index, uint divisor)
        {
            _glVertexAttribDivisor(index, divisor);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP1ui(uint index, uint type, bool normalized, uint value);
        internal static glVertexAttribP1ui _glVertexAttribP1ui;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP1ui", IsExtension=false)]
        public static void VertexAttribP1ui(uint index, uint type, bool normalized, uint value)
        {
            _glVertexAttribP1ui(index, type, normalized, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP1uiv(uint index, uint type, bool normalized, IntPtr value);
        internal static glVertexAttribP1uiv _glVertexAttribP1uiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP1uiv", IsExtension=false)]
        public static void VertexAttribP1uiv(uint index, uint type, bool normalized, IntPtr value)
        {
            _glVertexAttribP1uiv(index, type, normalized, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP2ui(uint index, uint type, bool normalized, uint value);
        internal static glVertexAttribP2ui _glVertexAttribP2ui;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP2ui", IsExtension=false)]
        public static void VertexAttribP2ui(uint index, uint type, bool normalized, uint value)
        {
            _glVertexAttribP2ui(index, type, normalized, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP2uiv(uint index, uint type, bool normalized, IntPtr value);
        internal static glVertexAttribP2uiv _glVertexAttribP2uiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP2uiv", IsExtension=false)]
        public static void VertexAttribP2uiv(uint index, uint type, bool normalized, IntPtr value)
        {
            _glVertexAttribP2uiv(index, type, normalized, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP3ui(uint index, uint type, bool normalized, uint value);
        internal static glVertexAttribP3ui _glVertexAttribP3ui;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP3ui", IsExtension=false)]
        public static void VertexAttribP3ui(uint index, uint type, bool normalized, uint value)
        {
            _glVertexAttribP3ui(index, type, normalized, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP3uiv(uint index, uint type, bool normalized, IntPtr value);
        internal static glVertexAttribP3uiv _glVertexAttribP3uiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP3uiv", IsExtension=false)]
        public static void VertexAttribP3uiv(uint index, uint type, bool normalized, IntPtr value)
        {
            _glVertexAttribP3uiv(index, type, normalized, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP4ui(uint index, uint type, bool normalized, uint value);
        internal static glVertexAttribP4ui _glVertexAttribP4ui;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP4ui", IsExtension=false)]
        public static void VertexAttribP4ui(uint index, uint type, bool normalized, uint value)
        {
            _glVertexAttribP4ui(index, type, normalized, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribP4uiv(uint index, uint type, bool normalized, IntPtr value);
        internal static glVertexAttribP4uiv _glVertexAttribP4uiv;

        [Version(Group="GL_VERSION_3_3", Version = "3.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribP4uiv", IsExtension=false)]
        public static void VertexAttribP4uiv(uint index, uint type, bool normalized, IntPtr value)
        {
            _glVertexAttribP4uiv(index, type, normalized, value);
        }
        

        #endregion

        #region GL_VERSION_4_0
        public const uint GL_SAMPLE_SHADING = (uint)0x8C36;
        public const uint GL_MIN_SAMPLE_SHADING_VALUE = (uint)0x8C37;
        public const uint GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET = (uint)0x8E5E;
        public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET = (uint)0x8E5F;
        public const uint GL_TEXTURE_CUBE_MAP_ARRAY = (uint)0x9009;
        public const uint GL_PROXY_TEXTURE_CUBE_MAP_ARRAY = (uint)0x900B;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY = (uint)0x900C;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW = (uint)0x900D;
        public const uint GL_INT_SAMPLER_CUBE_MAP_ARRAY = (uint)0x900E;
        public const uint GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY = (uint)0x900F;
        public const uint GL_DRAW_INDIRECT_BUFFER = (uint)0x8F3F;
        public const uint GL_DRAW_INDIRECT_BUFFER_BINDING = (uint)0x8F43;
        public const uint GL_GEOMETRY_SHADER_INVOCATIONS = (uint)0x887F;
        public const uint GL_MAX_GEOMETRY_SHADER_INVOCATIONS = (uint)0x8E5A;
        public const uint GL_MIN_FRAGMENT_INTERPOLATION_OFFSET = (uint)0x8E5B;
        public const uint GL_MAX_FRAGMENT_INTERPOLATION_OFFSET = (uint)0x8E5C;
        public const uint GL_FRAGMENT_INTERPOLATION_OFFSET_BITS = (uint)0x8E5D;
        public const uint GL_MAX_VERTEX_STREAMS = (uint)0x8E71;
        public const uint GL_DOUBLE_VEC2 = (uint)0x8FFC;
        public const uint GL_DOUBLE_VEC3 = (uint)0x8FFD;
        public const uint GL_DOUBLE_VEC4 = (uint)0x8FFE;
        public const uint GL_DOUBLE_MAT2 = (uint)0x8F46;
        public const uint GL_DOUBLE_MAT3 = (uint)0x8F47;
        public const uint GL_DOUBLE_MAT4 = (uint)0x8F48;
        public const uint GL_DOUBLE_MAT2x3 = (uint)0x8F49;
        public const uint GL_DOUBLE_MAT2x4 = (uint)0x8F4A;
        public const uint GL_DOUBLE_MAT3x2 = (uint)0x8F4B;
        public const uint GL_DOUBLE_MAT3x4 = (uint)0x8F4C;
        public const uint GL_DOUBLE_MAT4x2 = (uint)0x8F4D;
        public const uint GL_DOUBLE_MAT4x3 = (uint)0x8F4E;
        public const uint GL_ACTIVE_SUBROUTINES = (uint)0x8DE5;
        public const uint GL_ACTIVE_SUBROUTINE_UNIFORMS = (uint)0x8DE6;
        public const uint GL_ACTIVE_SUBROUTINE_UNIFORM_LOCATIONS = (uint)0x8E47;
        public const uint GL_ACTIVE_SUBROUTINE_MAX_LENGTH = (uint)0x8E48;
        public const uint GL_ACTIVE_SUBROUTINE_UNIFORM_MAX_LENGTH = (uint)0x8E49;
        public const uint GL_MAX_SUBROUTINES = (uint)0x8DE7;
        public const uint GL_MAX_SUBROUTINE_UNIFORM_LOCATIONS = (uint)0x8DE8;
        public const uint GL_NUM_COMPATIBLE_SUBROUTINES = (uint)0x8E4A;
        public const uint GL_COMPATIBLE_SUBROUTINES = (uint)0x8E4B;
        public const uint GL_PATCHES = (uint)0x000E;
        public const uint GL_PATCH_VERTICES = (uint)0x8E72;
        public const uint GL_PATCH_DEFAULT_INNER_LEVEL = (uint)0x8E73;
        public const uint GL_PATCH_DEFAULT_OUTER_LEVEL = (uint)0x8E74;
        public const uint GL_TESS_CONTROL_OUTPUT_VERTICES = (uint)0x8E75;
        public const uint GL_TESS_GEN_MODE = (uint)0x8E76;
        public const uint GL_TESS_GEN_SPACING = (uint)0x8E77;
        public const uint GL_TESS_GEN_VERTEX_ORDER = (uint)0x8E78;
        public const uint GL_TESS_GEN_POINT_MODE = (uint)0x8E79;
        public const uint GL_ISOLINES = (uint)0x8E7A;
        public const uint GL_QUADS = (uint)0x0007;
        public const uint GL_FRACTIONAL_ODD = (uint)0x8E7B;
        public const uint GL_FRACTIONAL_EVEN = (uint)0x8E7C;
        public const uint GL_MAX_PATCH_VERTICES = (uint)0x8E7D;
        public const uint GL_MAX_TESS_GEN_LEVEL = (uint)0x8E7E;
        public const uint GL_MAX_TESS_CONTROL_UNIFORM_COMPONENTS = (uint)0x8E7F;
        public const uint GL_MAX_TESS_EVALUATION_UNIFORM_COMPONENTS = (uint)0x8E80;
        public const uint GL_MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS = (uint)0x8E81;
        public const uint GL_MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS = (uint)0x8E82;
        public const uint GL_MAX_TESS_CONTROL_OUTPUT_COMPONENTS = (uint)0x8E83;
        public const uint GL_MAX_TESS_PATCH_COMPONENTS = (uint)0x8E84;
        public const uint GL_MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS = (uint)0x8E85;
        public const uint GL_MAX_TESS_EVALUATION_OUTPUT_COMPONENTS = (uint)0x8E86;
        public const uint GL_MAX_TESS_CONTROL_UNIFORM_BLOCKS = (uint)0x8E89;
        public const uint GL_MAX_TESS_EVALUATION_UNIFORM_BLOCKS = (uint)0x8E8A;
        public const uint GL_MAX_TESS_CONTROL_INPUT_COMPONENTS = (uint)0x886C;
        public const uint GL_MAX_TESS_EVALUATION_INPUT_COMPONENTS = (uint)0x886D;
        public const uint GL_MAX_COMBINED_TESS_CONTROL_UNIFORM_COMPONENTS = (uint)0x8E1E;
        public const uint GL_MAX_COMBINED_TESS_EVALUATION_UNIFORM_COMPONENTS = (uint)0x8E1F;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_CONTROL_SHADER = (uint)0x84F0;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_EVALUATION_SHADER = (uint)0x84F1;
        public const uint GL_TESS_EVALUATION_SHADER = (uint)0x8E87;
        public const uint GL_TESS_CONTROL_SHADER = (uint)0x8E88;
        public const uint GL_TRANSFORM_FEEDBACK = (uint)0x8E22;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_PAUSED = (uint)0x8E23;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_ACTIVE = (uint)0x8E24;
        public const uint GL_TRANSFORM_FEEDBACK_BINDING = (uint)0x8E25;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_BUFFERS = (uint)0x8E70;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMinSampleShading(float value);
        internal static glMinSampleShading _glMinSampleShading;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMinSampleShading", IsExtension=false)]
        public static void MinSampleShading(float value)
        {
            _glMinSampleShading(value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendEquationi(uint buf, uint mode);
        internal static glBlendEquationi _glBlendEquationi;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendEquationi", IsExtension=false)]
        public static void BlendEquationi(uint buf, uint mode)
        {
            _glBlendEquationi(buf, mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha);
        internal static glBlendEquationSeparatei _glBlendEquationSeparatei;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendEquationSeparatei", IsExtension=false)]
        public static void BlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha)
        {
            _glBlendEquationSeparatei(buf, modeRGB, modeAlpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendFunci(uint buf, uint src, uint dst);
        internal static glBlendFunci _glBlendFunci;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendFunci", IsExtension=false)]
        public static void BlendFunci(uint buf, uint src, uint dst)
        {
            _glBlendFunci(buf, src, dst);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);
        internal static glBlendFuncSeparatei _glBlendFuncSeparatei;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendFuncSeparatei", IsExtension=false)]
        public static void BlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
        {
            _glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawArraysIndirect(uint mode, IntPtr indirect);
        internal static glDrawArraysIndirect _glDrawArraysIndirect;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawArraysIndirect", IsExtension=false)]
        public static void DrawArraysIndirect(uint mode, IntPtr indirect)
        {
            _glDrawArraysIndirect(mode, indirect);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElementsIndirect(uint mode, uint type, IntPtr indirect);
        internal static glDrawElementsIndirect _glDrawElementsIndirect;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElementsIndirect", IsExtension=false)]
        public static void DrawElementsIndirect(uint mode, uint type, IntPtr indirect)
        {
            _glDrawElementsIndirect(mode, type, indirect);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1d(int location, double x);
        internal static glUniform1d _glUniform1d;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1d", IsExtension=false)]
        public static void Uniform1d(int location, double x)
        {
            _glUniform1d(location, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2d(int location, double x, double y);
        internal static glUniform2d _glUniform2d;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2d", IsExtension=false)]
        public static void Uniform2d(int location, double x, double y)
        {
            _glUniform2d(location, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3d(int location, double x, double y, double z);
        internal static glUniform3d _glUniform3d;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3d", IsExtension=false)]
        public static void Uniform3d(int location, double x, double y, double z)
        {
            _glUniform3d(location, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4d(int location, double x, double y, double z, double w);
        internal static glUniform4d _glUniform4d;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4d", IsExtension=false)]
        public static void Uniform4d(int location, double x, double y, double z, double w)
        {
            _glUniform4d(location, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1dv(int location, int count, double[] value);
        internal static glUniform1dv _glUniform1dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1dv", IsExtension=false)]
        public static void Uniform1dv(int location, int count, double[] value)
        {
            _glUniform1dv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2dv(int location, int count, double[] value);
        internal static glUniform2dv _glUniform2dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2dv", IsExtension=false)]
        public static void Uniform2dv(int location, int count, double[] value)
        {
            _glUniform2dv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3dv(int location, int count, double[] value);
        internal static glUniform3dv _glUniform3dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3dv", IsExtension=false)]
        public static void Uniform3dv(int location, int count, double[] value)
        {
            _glUniform3dv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4dv(int location, int count, double[] value);
        internal static glUniform4dv _glUniform4dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4dv", IsExtension=false)]
        public static void Uniform4dv(int location, int count, double[] value)
        {
            _glUniform4dv(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix2dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix2dv _glUniformMatrix2dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix2dv", IsExtension=false)]
        public static void UniformMatrix2dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix2dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix3dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix3dv _glUniformMatrix3dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix3dv", IsExtension=false)]
        public static void UniformMatrix3dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix3dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix4dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix4dv _glUniformMatrix4dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix4dv", IsExtension=false)]
        public static void UniformMatrix4dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix4dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix2x3dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix2x3dv _glUniformMatrix2x3dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix2x3dv", IsExtension=false)]
        public static void UniformMatrix2x3dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix2x3dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix2x4dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix2x4dv _glUniformMatrix2x4dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix2x4dv", IsExtension=false)]
        public static void UniformMatrix2x4dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix2x4dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix3x2dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix3x2dv _glUniformMatrix3x2dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix3x2dv", IsExtension=false)]
        public static void UniformMatrix3x2dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix3x2dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix3x4dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix3x4dv _glUniformMatrix3x4dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix3x4dv", IsExtension=false)]
        public static void UniformMatrix3x4dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix3x4dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix4x2dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix4x2dv _glUniformMatrix4x2dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix4x2dv", IsExtension=false)]
        public static void UniformMatrix4x2dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix4x2dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformMatrix4x3dv(int location, int count, bool transpose, double[] value);
        internal static glUniformMatrix4x3dv _glUniformMatrix4x3dv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformMatrix4x3dv", IsExtension=false)]
        public static void UniformMatrix4x3dv(int location, int count, bool transpose, double[] value)
        {
            _glUniformMatrix4x3dv(location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetUniformdv(uint program, int location, double[] @params);
        internal static glGetUniformdv _glGetUniformdv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformdv", IsExtension=false)]
        public static void GetUniformdv(uint program, int location, double[] @params)
        {
            _glGetUniformdv(program, location, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate int glGetSubroutineUniformLocation(uint program, uint shadertype, IntPtr name);
        internal static glGetSubroutineUniformLocation _glGetSubroutineUniformLocation;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetSubroutineUniformLocation", IsExtension=false)]
        public static int GetSubroutineUniformLocation(uint program, uint shadertype, IntPtr name)
        {
            int data = _glGetSubroutineUniformLocation(program, shadertype, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetSubroutineIndex(uint program, uint shadertype, IntPtr name);
        internal static glGetSubroutineIndex _glGetSubroutineIndex;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetSubroutineIndex", IsExtension=false)]
        public static uint GetSubroutineIndex(uint program, uint shadertype, IntPtr name)
        {
            uint data = _glGetSubroutineIndex(program, shadertype, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveSubroutineUniformiv(uint program, uint shadertype, uint index, uint pname, int[] values);
        internal static glGetActiveSubroutineUniformiv _glGetActiveSubroutineUniformiv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveSubroutineUniformiv", IsExtension=false)]
        public static void GetActiveSubroutineUniformiv(uint program, uint shadertype, uint index, uint pname, int[] values)
        {
            _glGetActiveSubroutineUniformiv(program, shadertype, index, pname, values);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveSubroutineUniformName(uint program, uint shadertype, uint index, int bufsize, IntPtr length, byte[] name);
        internal static glGetActiveSubroutineUniformName _glGetActiveSubroutineUniformName;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveSubroutineUniformName", IsExtension=false)]
        public static void GetActiveSubroutineUniformName(uint program, uint shadertype, uint index, int bufsize, IntPtr length, byte[] name)
        {
            _glGetActiveSubroutineUniformName(program, shadertype, index, bufsize, length, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveSubroutineName(uint program, uint shadertype, uint index, int bufsize, IntPtr length, byte[] name);
        internal static glGetActiveSubroutineName _glGetActiveSubroutineName;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveSubroutineName", IsExtension=false)]
        public static void GetActiveSubroutineName(uint program, uint shadertype, uint index, int bufsize, IntPtr length, byte[] name)
        {
            _glGetActiveSubroutineName(program, shadertype, index, bufsize, length, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformSubroutinesuiv(uint shadertype, int count, uint[] indices);
        internal static glUniformSubroutinesuiv _glUniformSubroutinesuiv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformSubroutinesuiv", IsExtension=false)]
        public static void UniformSubroutinesuiv(uint shadertype, int count, uint[] indices)
        {
            _glUniformSubroutinesuiv(shadertype, count, indices);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetUniformSubroutineuiv(uint shadertype, int location, IntPtr @params);
        internal static glGetUniformSubroutineuiv _glGetUniformSubroutineuiv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformSubroutineuiv", IsExtension=false)]
        public static void GetUniformSubroutineuiv(uint shadertype, int location, IntPtr @params)
        {
            _glGetUniformSubroutineuiv(shadertype, location, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramStageiv(uint program, uint shadertype, uint pname, IntPtr values);
        internal static glGetProgramStageiv _glGetProgramStageiv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramStageiv", IsExtension=false)]
        public static void GetProgramStageiv(uint program, uint shadertype, uint pname, IntPtr values)
        {
            _glGetProgramStageiv(program, shadertype, pname, values);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPatchParameteri(uint pname, int value);
        internal static glPatchParameteri _glPatchParameteri;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPatchParameteri", IsExtension=false)]
        public static void PatchParameteri(uint pname, int value)
        {
            _glPatchParameteri(pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPatchParameterfv(uint pname, float[] values);
        internal static glPatchParameterfv _glPatchParameterfv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPatchParameterfv", IsExtension=false)]
        public static void PatchParameterfv(uint pname, float[] values)
        {
            _glPatchParameterfv(pname, values);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindTransformFeedback(uint target, uint id);
        internal static glBindTransformFeedback _glBindTransformFeedback;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindTransformFeedback", IsExtension=false)]
        public static void BindTransformFeedback(uint target, uint id)
        {
            _glBindTransformFeedback(target, id);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteTransformFeedbacks(int n, uint[] ids);
        internal static glDeleteTransformFeedbacks _glDeleteTransformFeedbacks;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteTransformFeedbacks", IsExtension=false)]
        public static void DeleteTransformFeedbacks(int n, uint[] ids)
        {
            _glDeleteTransformFeedbacks(n, ids);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenTransformFeedbacks(int n, uint[] ids);
        internal static glGenTransformFeedbacks _glGenTransformFeedbacks;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenTransformFeedbacks", IsExtension=false)]
        public static void GenTransformFeedbacks(int n, uint[] ids)
        {
            _glGenTransformFeedbacks(n, ids);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsTransformFeedback(uint id);
        internal static glIsTransformFeedback _glIsTransformFeedback;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsTransformFeedback", IsExtension=false)]
        public static bool IsTransformFeedback(uint id)
        {
            bool data = _glIsTransformFeedback(id);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPauseTransformFeedback();
        internal static glPauseTransformFeedback _glPauseTransformFeedback;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPauseTransformFeedback", IsExtension=false)]
        public static void PauseTransformFeedback()
        {
            _glPauseTransformFeedback();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glResumeTransformFeedback();
        internal static glResumeTransformFeedback _glResumeTransformFeedback;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glResumeTransformFeedback", IsExtension=false)]
        public static void ResumeTransformFeedback()
        {
            _glResumeTransformFeedback();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawTransformFeedback(uint mode, uint id);
        internal static glDrawTransformFeedback _glDrawTransformFeedback;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawTransformFeedback", IsExtension=false)]
        public static void DrawTransformFeedback(uint mode, uint id)
        {
            _glDrawTransformFeedback(mode, id);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawTransformFeedbackStream(uint mode, uint id, uint stream);
        internal static glDrawTransformFeedbackStream _glDrawTransformFeedbackStream;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawTransformFeedbackStream", IsExtension=false)]
        public static void DrawTransformFeedbackStream(uint mode, uint id, uint stream)
        {
            _glDrawTransformFeedbackStream(mode, id, stream);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBeginQueryIndexed(uint target, uint index, uint id);
        internal static glBeginQueryIndexed _glBeginQueryIndexed;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBeginQueryIndexed", IsExtension=false)]
        public static void BeginQueryIndexed(uint target, uint index, uint id)
        {
            _glBeginQueryIndexed(target, index, id);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEndQueryIndexed(uint target, uint index);
        internal static glEndQueryIndexed _glEndQueryIndexed;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEndQueryIndexed", IsExtension=false)]
        public static void EndQueryIndexed(uint target, uint index)
        {
            _glEndQueryIndexed(target, index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryIndexediv(uint target, uint index, uint pname, int[] @params);
        internal static glGetQueryIndexediv _glGetQueryIndexediv;

        [Version(Group="GL_VERSION_4_0", Version = "4.0", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryIndexediv", IsExtension=false)]
        public static void GetQueryIndexediv(uint target, uint index, uint pname, int[] @params)
        {
            _glGetQueryIndexediv(target, index, pname, @params);
        }
        

        #endregion

        #region GL_VERSION_4_1
        public const uint GL_FIXED = (uint)0x140C;
        public const uint GL_IMPLEMENTATION_COLOR_READ_TYPE = (uint)0x8B9A;
        public const uint GL_IMPLEMENTATION_COLOR_READ_FORMAT = (uint)0x8B9B;
        public const uint GL_LOW_FLOAT = (uint)0x8DF0;
        public const uint GL_MEDIUM_FLOAT = (uint)0x8DF1;
        public const uint GL_HIGH_FLOAT = (uint)0x8DF2;
        public const uint GL_LOW_INT = (uint)0x8DF3;
        public const uint GL_MEDIUM_INT = (uint)0x8DF4;
        public const uint GL_HIGH_INT = (uint)0x8DF5;
        public const uint GL_SHADER_COMPILER = (uint)0x8DFA;
        public const uint GL_SHADER_BINARY_FORMATS = (uint)0x8DF8;
        public const uint GL_NUM_SHADER_BINARY_FORMATS = (uint)0x8DF9;
        public const uint GL_MAX_VERTEX_UNIFORM_VECTORS = (uint)0x8DFB;
        public const uint GL_MAX_VARYING_VECTORS = (uint)0x8DFC;
        public const uint GL_MAX_FRAGMENT_UNIFORM_VECTORS = (uint)0x8DFD;
        public const uint GL_RGB565 = (uint)0x8D62;
        public const uint GL_PROGRAM_BINARY_RETRIEVABLE_HINT = (uint)0x8257;
        public const uint GL_PROGRAM_BINARY_LENGTH = (uint)0x8741;
        public const uint GL_NUM_PROGRAM_BINARY_FORMATS = (uint)0x87FE;
        public const uint GL_PROGRAM_BINARY_FORMATS = (uint)0x87FF;
        public const uint GL_VERTEX_SHADER_BIT = (uint)0x00000001;
        public const uint GL_FRAGMENT_SHADER_BIT = (uint)0x00000002;
        public const uint GL_GEOMETRY_SHADER_BIT = (uint)0x00000004;
        public const uint GL_TESS_CONTROL_SHADER_BIT = (uint)0x00000008;
        public const uint GL_TESS_EVALUATION_SHADER_BIT = (uint)0x00000010;
        public const uint GL_ALL_SHADER_BITS = (uint)0xFFFFFFFF;
        public const uint GL_PROGRAM_SEPARABLE = (uint)0x8258;
        public const uint GL_ACTIVE_PROGRAM = (uint)0x8259;
        public const uint GL_PROGRAM_PIPELINE_BINDING = (uint)0x825A;
        public const uint GL_MAX_VIEWPORTS = (uint)0x825B;
        public const uint GL_VIEWPORT_SUBPIXEL_BITS = (uint)0x825C;
        public const uint GL_VIEWPORT_BOUNDS_RANGE = (uint)0x825D;
        public const uint GL_LAYER_PROVOKING_VERTEX = (uint)0x825E;
        public const uint GL_VIEWPORT_INDEX_PROVOKING_VERTEX = (uint)0x825F;
        public const uint GL_UNDEFINED_VERTEX = (uint)0x8260;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glReleaseShaderCompiler();
        internal static glReleaseShaderCompiler _glReleaseShaderCompiler;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glReleaseShaderCompiler", IsExtension=false)]
        public static void ReleaseShaderCompiler()
        {
            _glReleaseShaderCompiler();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glShaderBinary(int count, uint[] shaders, uint binaryformat, IntPtr binary, int length);
        internal static glShaderBinary _glShaderBinary;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glShaderBinary", IsExtension=false)]
        public static void ShaderBinary(int count, uint[] shaders, uint binaryformat, IntPtr binary, int length)
        {
            _glShaderBinary(count, shaders, binaryformat, binary, length);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, int[] range, int[] precision);
        internal static glGetShaderPrecisionFormat _glGetShaderPrecisionFormat;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetShaderPrecisionFormat", IsExtension=false)]
        public static void GetShaderPrecisionFormat(uint shadertype, uint precisiontype, int[] range, int[] precision)
        {
            _glGetShaderPrecisionFormat(shadertype, precisiontype, range, precision);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDepthRangef(float n, float f);
        internal static glDepthRangef _glDepthRangef;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDepthRangef", IsExtension=false)]
        public static void DepthRangef(float n, float f)
        {
            _glDepthRangef(n, f);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearDepthf(float d);
        internal static glClearDepthf _glClearDepthf;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearDepthf", IsExtension=false)]
        public static void ClearDepthf(float d)
        {
            _glClearDepthf(d);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramBinary(uint program, int bufSize, IntPtr length, IntPtr binaryFormat, IntPtr binary);
        internal static glGetProgramBinary _glGetProgramBinary;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramBinary", IsExtension=false)]
        public static void GetProgramBinary(uint program, int bufSize, IntPtr length, IntPtr binaryFormat, IntPtr binary)
        {
            _glGetProgramBinary(program, bufSize, length, binaryFormat, binary);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramBinary(uint program, uint binaryFormat, IntPtr binary, int length);
        internal static glProgramBinary _glProgramBinary;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramBinary", IsExtension=false)]
        public static void ProgramBinary(uint program, uint binaryFormat, IntPtr binary, int length)
        {
            _glProgramBinary(program, binaryFormat, binary, length);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramParameteri(uint program, uint pname, int value);
        internal static glProgramParameteri _glProgramParameteri;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramParameteri", IsExtension=false)]
        public static void ProgramParameteri(uint program, uint pname, int value)
        {
            _glProgramParameteri(program, pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUseProgramStages(uint pipeline, uint stages, uint program);
        internal static glUseProgramStages _glUseProgramStages;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUseProgramStages", IsExtension=false)]
        public static void UseProgramStages(uint pipeline, uint stages, uint program)
        {
            _glUseProgramStages(pipeline, stages, program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glActiveShaderProgram(uint pipeline, uint program);
        internal static glActiveShaderProgram _glActiveShaderProgram;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glActiveShaderProgram", IsExtension=false)]
        public static void ActiveShaderProgram(uint pipeline, uint program)
        {
            _glActiveShaderProgram(pipeline, program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glCreateShaderProgramv(uint type, int count, string[] strings);
        internal static glCreateShaderProgramv _glCreateShaderProgramv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateShaderProgramv", IsExtension=false)]
        public static uint CreateShaderProgramv(uint type, int count, string[] strings)
        {
            uint data = _glCreateShaderProgramv(type, count, strings);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindProgramPipeline(uint pipeline);
        internal static glBindProgramPipeline _glBindProgramPipeline;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindProgramPipeline", IsExtension=false)]
        public static void BindProgramPipeline(uint pipeline)
        {
            _glBindProgramPipeline(pipeline);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteProgramPipelines(int n, uint[] pipelines);
        internal static glDeleteProgramPipelines _glDeleteProgramPipelines;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteProgramPipelines", IsExtension=false)]
        public static void DeleteProgramPipelines(int n, uint[] pipelines)
        {
            _glDeleteProgramPipelines(n, pipelines);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenProgramPipelines(int n, uint[] pipelines);
        internal static glGenProgramPipelines _glGenProgramPipelines;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenProgramPipelines", IsExtension=false)]
        public static void GenProgramPipelines(int n, uint[] pipelines)
        {
            _glGenProgramPipelines(n, pipelines);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsProgramPipeline(uint pipeline);
        internal static glIsProgramPipeline _glIsProgramPipeline;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsProgramPipeline", IsExtension=false)]
        public static bool IsProgramPipeline(uint pipeline)
        {
            bool data = _glIsProgramPipeline(pipeline);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramPipelineiv(uint pipeline, uint pname, int[] @params);
        internal static glGetProgramPipelineiv _glGetProgramPipelineiv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramPipelineiv", IsExtension=false)]
        public static void GetProgramPipelineiv(uint pipeline, uint pname, int[] @params)
        {
            _glGetProgramPipelineiv(pipeline, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1i(uint program, int location, int v0);
        internal static glProgramUniform1i _glProgramUniform1i;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1i", IsExtension=false)]
        public static void ProgramUniform1i(uint program, int location, int v0)
        {
            _glProgramUniform1i(program, location, v0);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1iv(uint program, int location, int count, int[] value);
        internal static glProgramUniform1iv _glProgramUniform1iv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1iv", IsExtension=false)]
        public static void ProgramUniform1iv(uint program, int location, int count, int[] value)
        {
            _glProgramUniform1iv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1f(uint program, int location, float v0);
        internal static glProgramUniform1f _glProgramUniform1f;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1f", IsExtension=false)]
        public static void ProgramUniform1f(uint program, int location, float v0)
        {
            _glProgramUniform1f(program, location, v0);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1fv(uint program, int location, int count, float[] value);
        internal static glProgramUniform1fv _glProgramUniform1fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1fv", IsExtension=false)]
        public static void ProgramUniform1fv(uint program, int location, int count, float[] value)
        {
            _glProgramUniform1fv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1d(uint program, int location, double v0);
        internal static glProgramUniform1d _glProgramUniform1d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1d", IsExtension=false)]
        public static void ProgramUniform1d(uint program, int location, double v0)
        {
            _glProgramUniform1d(program, location, v0);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1dv(uint program, int location, int count, double[] value);
        internal static glProgramUniform1dv _glProgramUniform1dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1dv", IsExtension=false)]
        public static void ProgramUniform1dv(uint program, int location, int count, double[] value)
        {
            _glProgramUniform1dv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1ui(uint program, int location, uint v0);
        internal static glProgramUniform1ui _glProgramUniform1ui;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1ui", IsExtension=false)]
        public static void ProgramUniform1ui(uint program, int location, uint v0)
        {
            _glProgramUniform1ui(program, location, v0);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1uiv(uint program, int location, int count, uint[] value);
        internal static glProgramUniform1uiv _glProgramUniform1uiv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1uiv", IsExtension=false)]
        public static void ProgramUniform1uiv(uint program, int location, int count, uint[] value)
        {
            _glProgramUniform1uiv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2i(uint program, int location, int v0, int v1);
        internal static glProgramUniform2i _glProgramUniform2i;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2i", IsExtension=false)]
        public static void ProgramUniform2i(uint program, int location, int v0, int v1)
        {
            _glProgramUniform2i(program, location, v0, v1);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2iv(uint program, int location, int count, int[] value);
        internal static glProgramUniform2iv _glProgramUniform2iv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2iv", IsExtension=false)]
        public static void ProgramUniform2iv(uint program, int location, int count, int[] value)
        {
            _glProgramUniform2iv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2f(uint program, int location, float v0, float v1);
        internal static glProgramUniform2f _glProgramUniform2f;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2f", IsExtension=false)]
        public static void ProgramUniform2f(uint program, int location, float v0, float v1)
        {
            _glProgramUniform2f(program, location, v0, v1);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2fv(uint program, int location, int count, float[] value);
        internal static glProgramUniform2fv _glProgramUniform2fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2fv", IsExtension=false)]
        public static void ProgramUniform2fv(uint program, int location, int count, float[] value)
        {
            _glProgramUniform2fv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2d(uint program, int location, double v0, double v1);
        internal static glProgramUniform2d _glProgramUniform2d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2d", IsExtension=false)]
        public static void ProgramUniform2d(uint program, int location, double v0, double v1)
        {
            _glProgramUniform2d(program, location, v0, v1);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2dv(uint program, int location, int count, double[] value);
        internal static glProgramUniform2dv _glProgramUniform2dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2dv", IsExtension=false)]
        public static void ProgramUniform2dv(uint program, int location, int count, double[] value)
        {
            _glProgramUniform2dv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2ui(uint program, int location, uint v0, uint v1);
        internal static glProgramUniform2ui _glProgramUniform2ui;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2ui", IsExtension=false)]
        public static void ProgramUniform2ui(uint program, int location, uint v0, uint v1)
        {
            _glProgramUniform2ui(program, location, v0, v1);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2uiv(uint program, int location, int count, uint[] value);
        internal static glProgramUniform2uiv _glProgramUniform2uiv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2uiv", IsExtension=false)]
        public static void ProgramUniform2uiv(uint program, int location, int count, uint[] value)
        {
            _glProgramUniform2uiv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3i(uint program, int location, int v0, int v1, int v2);
        internal static glProgramUniform3i _glProgramUniform3i;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3i", IsExtension=false)]
        public static void ProgramUniform3i(uint program, int location, int v0, int v1, int v2)
        {
            _glProgramUniform3i(program, location, v0, v1, v2);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3iv(uint program, int location, int count, int[] value);
        internal static glProgramUniform3iv _glProgramUniform3iv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3iv", IsExtension=false)]
        public static void ProgramUniform3iv(uint program, int location, int count, int[] value)
        {
            _glProgramUniform3iv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3f(uint program, int location, float v0, float v1, float v2);
        internal static glProgramUniform3f _glProgramUniform3f;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3f", IsExtension=false)]
        public static void ProgramUniform3f(uint program, int location, float v0, float v1, float v2)
        {
            _glProgramUniform3f(program, location, v0, v1, v2);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3fv(uint program, int location, int count, float[] value);
        internal static glProgramUniform3fv _glProgramUniform3fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3fv", IsExtension=false)]
        public static void ProgramUniform3fv(uint program, int location, int count, float[] value)
        {
            _glProgramUniform3fv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3d(uint program, int location, double v0, double v1, double v2);
        internal static glProgramUniform3d _glProgramUniform3d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3d", IsExtension=false)]
        public static void ProgramUniform3d(uint program, int location, double v0, double v1, double v2)
        {
            _glProgramUniform3d(program, location, v0, v1, v2);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3dv(uint program, int location, int count, double[] value);
        internal static glProgramUniform3dv _glProgramUniform3dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3dv", IsExtension=false)]
        public static void ProgramUniform3dv(uint program, int location, int count, double[] value)
        {
            _glProgramUniform3dv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3ui(uint program, int location, uint v0, uint v1, uint v2);
        internal static glProgramUniform3ui _glProgramUniform3ui;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3ui", IsExtension=false)]
        public static void ProgramUniform3ui(uint program, int location, uint v0, uint v1, uint v2)
        {
            _glProgramUniform3ui(program, location, v0, v1, v2);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3uiv(uint program, int location, int count, uint[] value);
        internal static glProgramUniform3uiv _glProgramUniform3uiv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3uiv", IsExtension=false)]
        public static void ProgramUniform3uiv(uint program, int location, int count, uint[] value)
        {
            _glProgramUniform3uiv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3);
        internal static glProgramUniform4i _glProgramUniform4i;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4i", IsExtension=false)]
        public static void ProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3)
        {
            _glProgramUniform4i(program, location, v0, v1, v2, v3);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4iv(uint program, int location, int count, int[] value);
        internal static glProgramUniform4iv _glProgramUniform4iv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4iv", IsExtension=false)]
        public static void ProgramUniform4iv(uint program, int location, int count, int[] value)
        {
            _glProgramUniform4iv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3);
        internal static glProgramUniform4f _glProgramUniform4f;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4f", IsExtension=false)]
        public static void ProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3)
        {
            _glProgramUniform4f(program, location, v0, v1, v2, v3);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4fv(uint program, int location, int count, float[] value);
        internal static glProgramUniform4fv _glProgramUniform4fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4fv", IsExtension=false)]
        public static void ProgramUniform4fv(uint program, int location, int count, float[] value)
        {
            _glProgramUniform4fv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4d(uint program, int location, double v0, double v1, double v2, double v3);
        internal static glProgramUniform4d _glProgramUniform4d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4d", IsExtension=false)]
        public static void ProgramUniform4d(uint program, int location, double v0, double v1, double v2, double v3)
        {
            _glProgramUniform4d(program, location, v0, v1, v2, v3);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4dv(uint program, int location, int count, double[] value);
        internal static glProgramUniform4dv _glProgramUniform4dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4dv", IsExtension=false)]
        public static void ProgramUniform4dv(uint program, int location, int count, double[] value)
        {
            _glProgramUniform4dv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4ui(uint program, int location, uint v0, uint v1, uint v2, uint v3);
        internal static glProgramUniform4ui _glProgramUniform4ui;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4ui", IsExtension=false)]
        public static void ProgramUniform4ui(uint program, int location, uint v0, uint v1, uint v2, uint v3)
        {
            _glProgramUniform4ui(program, location, v0, v1, v2, v3);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4uiv(uint program, int location, int count, uint[] value);
        internal static glProgramUniform4uiv _glProgramUniform4uiv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4uiv", IsExtension=false)]
        public static void ProgramUniform4uiv(uint program, int location, int count, uint[] value)
        {
            _glProgramUniform4uiv(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix2fv _glProgramUniformMatrix2fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix2fv", IsExtension=false)]
        public static void ProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix2fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix3fv _glProgramUniformMatrix3fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix3fv", IsExtension=false)]
        public static void ProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix3fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix4fv _glProgramUniformMatrix4fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix4fv", IsExtension=false)]
        public static void ProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix4fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix2dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix2dv _glProgramUniformMatrix2dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix2dv", IsExtension=false)]
        public static void ProgramUniformMatrix2dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix2dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix3dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix3dv _glProgramUniformMatrix3dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix3dv", IsExtension=false)]
        public static void ProgramUniformMatrix3dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix3dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix4dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix4dv _glProgramUniformMatrix4dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix4dv", IsExtension=false)]
        public static void ProgramUniformMatrix4dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix4dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix2x3fv _glProgramUniformMatrix2x3fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix2x3fv", IsExtension=false)]
        public static void ProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix2x3fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix3x2fv _glProgramUniformMatrix3x2fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix3x2fv", IsExtension=false)]
        public static void ProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix3x2fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix2x4fv _glProgramUniformMatrix2x4fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix2x4fv", IsExtension=false)]
        public static void ProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix2x4fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix4x2fv _glProgramUniformMatrix4x2fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix4x2fv", IsExtension=false)]
        public static void ProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix4x2fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix3x4fv _glProgramUniformMatrix3x4fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix3x4fv", IsExtension=false)]
        public static void ProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix3x4fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, float[] value);
        internal static glProgramUniformMatrix4x3fv _glProgramUniformMatrix4x3fv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix4x3fv", IsExtension=false)]
        public static void ProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, float[] value)
        {
            _glProgramUniformMatrix4x3fv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix2x3dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix2x3dv _glProgramUniformMatrix2x3dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix2x3dv", IsExtension=false)]
        public static void ProgramUniformMatrix2x3dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix2x3dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix3x2dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix3x2dv _glProgramUniformMatrix3x2dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix3x2dv", IsExtension=false)]
        public static void ProgramUniformMatrix3x2dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix3x2dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix2x4dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix2x4dv _glProgramUniformMatrix2x4dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix2x4dv", IsExtension=false)]
        public static void ProgramUniformMatrix2x4dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix2x4dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix4x2dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix4x2dv _glProgramUniformMatrix4x2dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix4x2dv", IsExtension=false)]
        public static void ProgramUniformMatrix4x2dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix4x2dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix3x4dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix3x4dv _glProgramUniformMatrix3x4dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix3x4dv", IsExtension=false)]
        public static void ProgramUniformMatrix3x4dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix3x4dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformMatrix4x3dv(uint program, int location, int count, bool transpose, double[] value);
        internal static glProgramUniformMatrix4x3dv _glProgramUniformMatrix4x3dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformMatrix4x3dv", IsExtension=false)]
        public static void ProgramUniformMatrix4x3dv(uint program, int location, int count, bool transpose, double[] value)
        {
            _glProgramUniformMatrix4x3dv(program, location, count, transpose, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glValidateProgramPipeline(uint pipeline);
        internal static glValidateProgramPipeline _glValidateProgramPipeline;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glValidateProgramPipeline", IsExtension=false)]
        public static void ValidateProgramPipeline(uint pipeline)
        {
            _glValidateProgramPipeline(pipeline);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramPipelineInfoLog(uint pipeline, int bufSize, IntPtr length, byte[] infoLog);
        internal static glGetProgramPipelineInfoLog _glGetProgramPipelineInfoLog;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramPipelineInfoLog", IsExtension=false)]
        public static void GetProgramPipelineInfoLog(uint pipeline, int bufSize, IntPtr length, byte[] infoLog)
        {
            _glGetProgramPipelineInfoLog(pipeline, bufSize, length, infoLog);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL1d(uint index, double x);
        internal static glVertexAttribL1d _glVertexAttribL1d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL1d", IsExtension=false)]
        public static void VertexAttribL1d(uint index, double x)
        {
            _glVertexAttribL1d(index, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL2d(uint index, double x, double y);
        internal static glVertexAttribL2d _glVertexAttribL2d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL2d", IsExtension=false)]
        public static void VertexAttribL2d(uint index, double x, double y)
        {
            _glVertexAttribL2d(index, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL3d(uint index, double x, double y, double z);
        internal static glVertexAttribL3d _glVertexAttribL3d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL3d", IsExtension=false)]
        public static void VertexAttribL3d(uint index, double x, double y, double z)
        {
            _glVertexAttribL3d(index, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL4d(uint index, double x, double y, double z, double w);
        internal static glVertexAttribL4d _glVertexAttribL4d;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL4d", IsExtension=false)]
        public static void VertexAttribL4d(uint index, double x, double y, double z, double w)
        {
            _glVertexAttribL4d(index, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL1dv(uint index, IntPtr v);
        internal static glVertexAttribL1dv _glVertexAttribL1dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL1dv", IsExtension=false)]
        public static void VertexAttribL1dv(uint index, IntPtr v)
        {
            _glVertexAttribL1dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL2dv(uint index, double[] v);
        internal static glVertexAttribL2dv _glVertexAttribL2dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL2dv", IsExtension=false)]
        public static void VertexAttribL2dv(uint index, double[] v)
        {
            _glVertexAttribL2dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL3dv(uint index, double[] v);
        internal static glVertexAttribL3dv _glVertexAttribL3dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL3dv", IsExtension=false)]
        public static void VertexAttribL3dv(uint index, double[] v)
        {
            _glVertexAttribL3dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL4dv(uint index, double[] v);
        internal static glVertexAttribL4dv _glVertexAttribL4dv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL4dv", IsExtension=false)]
        public static void VertexAttribL4dv(uint index, double[] v)
        {
            _glVertexAttribL4dv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribLPointer(uint index, int size, uint type, int stride, IntPtr pointer);
        internal static glVertexAttribLPointer _glVertexAttribLPointer;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribLPointer", IsExtension=false)]
        public static void VertexAttribLPointer(uint index, int size, uint type, int stride, IntPtr pointer)
        {
            _glVertexAttribLPointer(index, size, type, stride, pointer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribLdv(uint index, uint pname, double[] @params);
        internal static glGetVertexAttribLdv _glGetVertexAttribLdv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribLdv", IsExtension=false)]
        public static void GetVertexAttribLdv(uint index, uint pname, double[] @params)
        {
            _glGetVertexAttribLdv(index, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glViewportArrayv(uint first, int count, float[] v);
        internal static glViewportArrayv _glViewportArrayv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glViewportArrayv", IsExtension=false)]
        public static void ViewportArrayv(uint first, int count, float[] v)
        {
            _glViewportArrayv(first, count, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glViewportIndexedf(uint index, float x, float y, float w, float h);
        internal static glViewportIndexedf _glViewportIndexedf;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glViewportIndexedf", IsExtension=false)]
        public static void ViewportIndexedf(uint index, float x, float y, float w, float h)
        {
            _glViewportIndexedf(index, x, y, w, h);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glViewportIndexedfv(uint index, float[] v);
        internal static glViewportIndexedfv _glViewportIndexedfv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glViewportIndexedfv", IsExtension=false)]
        public static void ViewportIndexedfv(uint index, float[] v)
        {
            _glViewportIndexedfv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glScissorArrayv(uint first, int count, int[] v);
        internal static glScissorArrayv _glScissorArrayv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glScissorArrayv", IsExtension=false)]
        public static void ScissorArrayv(uint first, int count, int[] v)
        {
            _glScissorArrayv(first, count, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glScissorIndexed(uint index, int left, int bottom, int width, int height);
        internal static glScissorIndexed _glScissorIndexed;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glScissorIndexed", IsExtension=false)]
        public static void ScissorIndexed(uint index, int left, int bottom, int width, int height)
        {
            _glScissorIndexed(index, left, bottom, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glScissorIndexedv(uint index, int[] v);
        internal static glScissorIndexedv _glScissorIndexedv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glScissorIndexedv", IsExtension=false)]
        public static void ScissorIndexedv(uint index, int[] v)
        {
            _glScissorIndexedv(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDepthRangeArrayv(uint first, int count, double[] v);
        internal static glDepthRangeArrayv _glDepthRangeArrayv;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDepthRangeArrayv", IsExtension=false)]
        public static void DepthRangeArrayv(uint first, int count, double[] v)
        {
            _glDepthRangeArrayv(first, count, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDepthRangeIndexed(uint index, double n, double f);
        internal static glDepthRangeIndexed _glDepthRangeIndexed;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDepthRangeIndexed", IsExtension=false)]
        public static void DepthRangeIndexed(uint index, double n, double f)
        {
            _glDepthRangeIndexed(index, n, f);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetFloati_v(uint target, uint index, float[] data);
        internal static glGetFloati_v _glGetFloati_v;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetFloati_v", IsExtension=false)]
        public static void GetFloati_v(uint target, uint index, float[] data)
        {
            _glGetFloati_v(target, index, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetDoublei_v(uint target, uint index, double[] data);
        internal static glGetDoublei_v _glGetDoublei_v;

        [Version(Group="GL_VERSION_4_1", Version = "4.1", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetDoublei_v", IsExtension=false)]
        public static void GetDoublei_v(uint target, uint index, double[] data)
        {
            _glGetDoublei_v(target, index, data);
        }
        

        #endregion

        #region GL_VERSION_4_2
        public const uint GL_COPY_READ_BUFFER_BINDING = (uint)0x8F36;
        public const uint GL_COPY_WRITE_BUFFER_BINDING = (uint)0x8F37;
        public const uint GL_TRANSFORM_FEEDBACK_ACTIVE = (uint)0x8E24;
        public const uint GL_TRANSFORM_FEEDBACK_PAUSED = (uint)0x8E23;
        public const uint GL_UNPACK_COMPRESSED_BLOCK_WIDTH = (uint)0x9127;
        public const uint GL_UNPACK_COMPRESSED_BLOCK_HEIGHT = (uint)0x9128;
        public const uint GL_UNPACK_COMPRESSED_BLOCK_DEPTH = (uint)0x9129;
        public const uint GL_UNPACK_COMPRESSED_BLOCK_SIZE = (uint)0x912A;
        public const uint GL_PACK_COMPRESSED_BLOCK_WIDTH = (uint)0x912B;
        public const uint GL_PACK_COMPRESSED_BLOCK_HEIGHT = (uint)0x912C;
        public const uint GL_PACK_COMPRESSED_BLOCK_DEPTH = (uint)0x912D;
        public const uint GL_PACK_COMPRESSED_BLOCK_SIZE = (uint)0x912E;
        public const uint GL_NUM_SAMPLE_COUNTS = (uint)0x9380;
        public const uint GL_MIN_MAP_BUFFER_ALIGNMENT = (uint)0x90BC;
        public const uint GL_ATOMIC_COUNTER_BUFFER = (uint)0x92C0;
        public const uint GL_ATOMIC_COUNTER_BUFFER_BINDING = (uint)0x92C1;
        public const uint GL_ATOMIC_COUNTER_BUFFER_START = (uint)0x92C2;
        public const uint GL_ATOMIC_COUNTER_BUFFER_SIZE = (uint)0x92C3;
        public const uint GL_ATOMIC_COUNTER_BUFFER_DATA_SIZE = (uint)0x92C4;
        public const uint GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTERS = (uint)0x92C5;
        public const uint GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTER_INDICES = (uint)0x92C6;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_VERTEX_SHADER = (uint)0x92C7;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_CONTROL_SHADER = (uint)0x92C8;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_EVALUATION_SHADER = (uint)0x92C9;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_GEOMETRY_SHADER = (uint)0x92CA;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_FRAGMENT_SHADER = (uint)0x92CB;
        public const uint GL_MAX_VERTEX_ATOMIC_COUNTER_BUFFERS = (uint)0x92CC;
        public const uint GL_MAX_TESS_CONTROL_ATOMIC_COUNTER_BUFFERS = (uint)0x92CD;
        public const uint GL_MAX_TESS_EVALUATION_ATOMIC_COUNTER_BUFFERS = (uint)0x92CE;
        public const uint GL_MAX_GEOMETRY_ATOMIC_COUNTER_BUFFERS = (uint)0x92CF;
        public const uint GL_MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS = (uint)0x92D0;
        public const uint GL_MAX_COMBINED_ATOMIC_COUNTER_BUFFERS = (uint)0x92D1;
        public const uint GL_MAX_VERTEX_ATOMIC_COUNTERS = (uint)0x92D2;
        public const uint GL_MAX_TESS_CONTROL_ATOMIC_COUNTERS = (uint)0x92D3;
        public const uint GL_MAX_TESS_EVALUATION_ATOMIC_COUNTERS = (uint)0x92D4;
        public const uint GL_MAX_GEOMETRY_ATOMIC_COUNTERS = (uint)0x92D5;
        public const uint GL_MAX_FRAGMENT_ATOMIC_COUNTERS = (uint)0x92D6;
        public const uint GL_MAX_COMBINED_ATOMIC_COUNTERS = (uint)0x92D7;
        public const uint GL_MAX_ATOMIC_COUNTER_BUFFER_SIZE = (uint)0x92D8;
        public const uint GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS = (uint)0x92DC;
        public const uint GL_ACTIVE_ATOMIC_COUNTER_BUFFERS = (uint)0x92D9;
        public const uint GL_UNIFORM_ATOMIC_COUNTER_BUFFER_INDEX = (uint)0x92DA;
        public const uint GL_UNSIGNED_INT_ATOMIC_COUNTER = (uint)0x92DB;
        public const uint GL_VERTEX_ATTRIB_ARRAY_BARRIER_BIT = (uint)0x00000001;
        public const uint GL_ELEMENT_ARRAY_BARRIER_BIT = (uint)0x00000002;
        public const uint GL_UNIFORM_BARRIER_BIT = (uint)0x00000004;
        public const uint GL_TEXTURE_FETCH_BARRIER_BIT = (uint)0x00000008;
        public const uint GL_SHADER_IMAGE_ACCESS_BARRIER_BIT = (uint)0x00000020;
        public const uint GL_COMMAND_BARRIER_BIT = (uint)0x00000040;
        public const uint GL_PIXEL_BUFFER_BARRIER_BIT = (uint)0x00000080;
        public const uint GL_TEXTURE_UPDATE_BARRIER_BIT = (uint)0x00000100;
        public const uint GL_BUFFER_UPDATE_BARRIER_BIT = (uint)0x00000200;
        public const uint GL_FRAMEBUFFER_BARRIER_BIT = (uint)0x00000400;
        public const uint GL_TRANSFORM_FEEDBACK_BARRIER_BIT = (uint)0x00000800;
        public const uint GL_ATOMIC_COUNTER_BARRIER_BIT = (uint)0x00001000;
        public const uint GL_ALL_BARRIER_BITS = (uint)0xFFFFFFFF;
        public const uint GL_MAX_IMAGE_UNITS = (uint)0x8F38;
        public const uint GL_MAX_COMBINED_IMAGE_UNITS_AND_FRAGMENT_OUTPUTS = (uint)0x8F39;
        public const uint GL_IMAGE_BINDING_NAME = (uint)0x8F3A;
        public const uint GL_IMAGE_BINDING_LEVEL = (uint)0x8F3B;
        public const uint GL_IMAGE_BINDING_LAYERED = (uint)0x8F3C;
        public const uint GL_IMAGE_BINDING_LAYER = (uint)0x8F3D;
        public const uint GL_IMAGE_BINDING_ACCESS = (uint)0x8F3E;
        public const uint GL_IMAGE_1D = (uint)0x904C;
        public const uint GL_IMAGE_2D = (uint)0x904D;
        public const uint GL_IMAGE_3D = (uint)0x904E;
        public const uint GL_IMAGE_2D_RECT = (uint)0x904F;
        public const uint GL_IMAGE_CUBE = (uint)0x9050;
        public const uint GL_IMAGE_BUFFER = (uint)0x9051;
        public const uint GL_IMAGE_1D_ARRAY = (uint)0x9052;
        public const uint GL_IMAGE_2D_ARRAY = (uint)0x9053;
        public const uint GL_IMAGE_CUBE_MAP_ARRAY = (uint)0x9054;
        public const uint GL_IMAGE_2D_MULTISAMPLE = (uint)0x9055;
        public const uint GL_IMAGE_2D_MULTISAMPLE_ARRAY = (uint)0x9056;
        public const uint GL_INT_IMAGE_1D = (uint)0x9057;
        public const uint GL_INT_IMAGE_2D = (uint)0x9058;
        public const uint GL_INT_IMAGE_3D = (uint)0x9059;
        public const uint GL_INT_IMAGE_2D_RECT = (uint)0x905A;
        public const uint GL_INT_IMAGE_CUBE = (uint)0x905B;
        public const uint GL_INT_IMAGE_BUFFER = (uint)0x905C;
        public const uint GL_INT_IMAGE_1D_ARRAY = (uint)0x905D;
        public const uint GL_INT_IMAGE_2D_ARRAY = (uint)0x905E;
        public const uint GL_INT_IMAGE_CUBE_MAP_ARRAY = (uint)0x905F;
        public const uint GL_INT_IMAGE_2D_MULTISAMPLE = (uint)0x9060;
        public const uint GL_INT_IMAGE_2D_MULTISAMPLE_ARRAY = (uint)0x9061;
        public const uint GL_UNSIGNED_INT_IMAGE_1D = (uint)0x9062;
        public const uint GL_UNSIGNED_INT_IMAGE_2D = (uint)0x9063;
        public const uint GL_UNSIGNED_INT_IMAGE_3D = (uint)0x9064;
        public const uint GL_UNSIGNED_INT_IMAGE_2D_RECT = (uint)0x9065;
        public const uint GL_UNSIGNED_INT_IMAGE_CUBE = (uint)0x9066;
        public const uint GL_UNSIGNED_INT_IMAGE_BUFFER = (uint)0x9067;
        public const uint GL_UNSIGNED_INT_IMAGE_1D_ARRAY = (uint)0x9068;
        public const uint GL_UNSIGNED_INT_IMAGE_2D_ARRAY = (uint)0x9069;
        public const uint GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY = (uint)0x906A;
        public const uint GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE = (uint)0x906B;
        public const uint GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY = (uint)0x906C;
        public const uint GL_MAX_IMAGE_SAMPLES = (uint)0x906D;
        public const uint GL_IMAGE_BINDING_FORMAT = (uint)0x906E;
        public const uint GL_IMAGE_FORMAT_COMPATIBILITY_TYPE = (uint)0x90C7;
        public const uint GL_IMAGE_FORMAT_COMPATIBILITY_BY_SIZE = (uint)0x90C8;
        public const uint GL_IMAGE_FORMAT_COMPATIBILITY_BY_CLASS = (uint)0x90C9;
        public const uint GL_MAX_VERTEX_IMAGE_UNIFORMS = (uint)0x90CA;
        public const uint GL_MAX_TESS_CONTROL_IMAGE_UNIFORMS = (uint)0x90CB;
        public const uint GL_MAX_TESS_EVALUATION_IMAGE_UNIFORMS = (uint)0x90CC;
        public const uint GL_MAX_GEOMETRY_IMAGE_UNIFORMS = (uint)0x90CD;
        public const uint GL_MAX_FRAGMENT_IMAGE_UNIFORMS = (uint)0x90CE;
        public const uint GL_MAX_COMBINED_IMAGE_UNIFORMS = (uint)0x90CF;
        public const uint GL_COMPRESSED_RGBA_BPTC_UNORM = (uint)0x8E8C;
        public const uint GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM = (uint)0x8E8D;
        public const uint GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT = (uint)0x8E8E;
        public const uint GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT = (uint)0x8E8F;
        public const uint GL_TEXTURE_IMMUTABLE_FORMAT = (uint)0x912F;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawArraysInstancedBaseInstance(uint mode, int first, int count, int instancecount, uint baseinstance);
        internal static glDrawArraysInstancedBaseInstance _glDrawArraysInstancedBaseInstance;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawArraysInstancedBaseInstance", IsExtension=false)]
        public static void DrawArraysInstancedBaseInstance(uint mode, int first, int count, int instancecount, uint baseinstance)
        {
            _glDrawArraysInstancedBaseInstance(mode, first, count, instancecount, baseinstance);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElementsInstancedBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, uint baseinstance);
        internal static glDrawElementsInstancedBaseInstance _glDrawElementsInstancedBaseInstance;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElementsInstancedBaseInstance", IsExtension=false)]
        public static void DrawElementsInstancedBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, uint baseinstance)
        {
            _glDrawElementsInstancedBaseInstance(mode, count, type, indices, instancecount, baseinstance);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElementsInstancedBaseVertexBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex, uint baseinstance);
        internal static glDrawElementsInstancedBaseVertexBaseInstance _glDrawElementsInstancedBaseVertexBaseInstance;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElementsInstancedBaseVertexBaseInstance", IsExtension=false)]
        public static void DrawElementsInstancedBaseVertexBaseInstance(uint mode, int count, uint type, IntPtr indices, int instancecount, int basevertex, uint baseinstance)
        {
            _glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices, instancecount, basevertex, baseinstance);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetInternalformativ(uint target, uint internalformat, uint pname, int bufSize, int[] @params);
        internal static glGetInternalformativ _glGetInternalformativ;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetInternalformativ", IsExtension=false)]
        public static void GetInternalformativ(uint target, uint internalformat, uint pname, int bufSize, int[] @params)
        {
            _glGetInternalformativ(target, internalformat, pname, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, int[] @params);
        internal static glGetActiveAtomicCounterBufferiv _glGetActiveAtomicCounterBufferiv;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetActiveAtomicCounterBufferiv", IsExtension=false)]
        public static void GetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, int[] @params)
        {
            _glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindImageTexture(uint unit, uint texture, int level, bool layered, int layer, uint access, uint format);
        internal static glBindImageTexture _glBindImageTexture;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindImageTexture", IsExtension=false)]
        public static void BindImageTexture(uint unit, uint texture, int level, bool layered, int layer, uint access, uint format)
        {
            _glBindImageTexture(unit, texture, level, layered, layer, access, format);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMemoryBarrier(uint barriers);
        internal static glMemoryBarrier _glMemoryBarrier;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMemoryBarrier", IsExtension=false)]
        public static void MemoryBarrier(uint barriers)
        {
            _glMemoryBarrier(barriers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexStorage1D(uint target, int levels, uint internalformat, int width);
        internal static glTexStorage1D _glTexStorage1D;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexStorage1D", IsExtension=false)]
        public static void TexStorage1D(uint target, int levels, uint internalformat, int width)
        {
            _glTexStorage1D(target, levels, internalformat, width);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexStorage2D(uint target, int levels, uint internalformat, int width, int height);
        internal static glTexStorage2D _glTexStorage2D;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexStorage2D", IsExtension=false)]
        public static void TexStorage2D(uint target, int levels, uint internalformat, int width, int height)
        {
            _glTexStorage2D(target, levels, internalformat, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth);
        internal static glTexStorage3D _glTexStorage3D;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexStorage3D", IsExtension=false)]
        public static void TexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth)
        {
            _glTexStorage3D(target, levels, internalformat, width, height, depth);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawTransformFeedbackInstanced(uint mode, uint id, int instancecount);
        internal static glDrawTransformFeedbackInstanced _glDrawTransformFeedbackInstanced;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawTransformFeedbackInstanced", IsExtension=false)]
        public static void DrawTransformFeedbackInstanced(uint mode, uint id, int instancecount)
        {
            _glDrawTransformFeedbackInstanced(mode, id, instancecount);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawTransformFeedbackStreamInstanced(uint mode, uint id, uint stream, int instancecount);
        internal static glDrawTransformFeedbackStreamInstanced _glDrawTransformFeedbackStreamInstanced;

        [Version(Group="GL_VERSION_4_2", Version = "4.2", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawTransformFeedbackStreamInstanced", IsExtension=false)]
        public static void DrawTransformFeedbackStreamInstanced(uint mode, uint id, uint stream, int instancecount)
        {
            _glDrawTransformFeedbackStreamInstanced(mode, id, stream, instancecount);
        }
        

        #endregion

        #region GL_VERSION_4_3
        public const uint GL_NUM_SHADING_LANGUAGE_VERSIONS = (uint)0x82E9;
        public const uint GL_VERTEX_ATTRIB_ARRAY_LONG = (uint)0x874E;
        public const uint GL_COMPRESSED_RGB8_ETC2 = (uint)0x9274;
        public const uint GL_COMPRESSED_SRGB8_ETC2 = (uint)0x9275;
        public const uint GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = (uint)0x9276;
        public const uint GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = (uint)0x9277;
        public const uint GL_COMPRESSED_RGBA8_ETC2_EAC = (uint)0x9278;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC = (uint)0x9279;
        public const uint GL_COMPRESSED_R11_EAC = (uint)0x9270;
        public const uint GL_COMPRESSED_SIGNED_R11_EAC = (uint)0x9271;
        public const uint GL_COMPRESSED_RG11_EAC = (uint)0x9272;
        public const uint GL_COMPRESSED_SIGNED_RG11_EAC = (uint)0x9273;
        public const uint GL_PRIMITIVE_RESTART_FIXED_INDEX = (uint)0x8D69;
        public const uint GL_ANY_SAMPLES_PASSED_CONSERVATIVE = (uint)0x8D6A;
        public const uint GL_MAX_ELEMENT_INDEX = (uint)0x8D6B;
        public const uint GL_COMPUTE_SHADER = (uint)0x91B9;
        public const uint GL_MAX_COMPUTE_UNIFORM_BLOCKS = (uint)0x91BB;
        public const uint GL_MAX_COMPUTE_TEXTURE_IMAGE_UNITS = (uint)0x91BC;
        public const uint GL_MAX_COMPUTE_IMAGE_UNIFORMS = (uint)0x91BD;
        public const uint GL_MAX_COMPUTE_SHARED_MEMORY_SIZE = (uint)0x8262;
        public const uint GL_MAX_COMPUTE_UNIFORM_COMPONENTS = (uint)0x8263;
        public const uint GL_MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS = (uint)0x8264;
        public const uint GL_MAX_COMPUTE_ATOMIC_COUNTERS = (uint)0x8265;
        public const uint GL_MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS = (uint)0x8266;
        public const uint GL_MAX_COMPUTE_WORK_GROUP_INVOCATIONS = (uint)0x90EB;
        public const uint GL_MAX_COMPUTE_WORK_GROUP_COUNT = (uint)0x91BE;
        public const uint GL_MAX_COMPUTE_WORK_GROUP_SIZE = (uint)0x91BF;
        public const uint GL_COMPUTE_WORK_GROUP_SIZE = (uint)0x8267;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER = (uint)0x90EC;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER = (uint)0x90ED;
        public const uint GL_DISPATCH_INDIRECT_BUFFER = (uint)0x90EE;
        public const uint GL_DISPATCH_INDIRECT_BUFFER_BINDING = (uint)0x90EF;
        public const uint GL_COMPUTE_SHADER_BIT = (uint)0x00000020;
        public const uint GL_DEBUG_OUTPUT_SYNCHRONOUS = (uint)0x8242;
        public const uint GL_DEBUG_NEXT_LOGGED_MESSAGE_LENGTH = (uint)0x8243;
        public const uint GL_DEBUG_CALLBACK_FUNCTION = (uint)0x8244;
        public const uint GL_DEBUG_CALLBACK_USER_PARAM = (uint)0x8245;
        public const uint GL_DEBUG_SOURCE_API = (uint)0x8246;
        public const uint GL_DEBUG_SOURCE_WINDOW_SYSTEM = (uint)0x8247;
        public const uint GL_DEBUG_SOURCE_SHADER_COMPILER = (uint)0x8248;
        public const uint GL_DEBUG_SOURCE_THIRD_PARTY = (uint)0x8249;
        public const uint GL_DEBUG_SOURCE_APPLICATION = (uint)0x824A;
        public const uint GL_DEBUG_SOURCE_OTHER = (uint)0x824B;
        public const uint GL_DEBUG_TYPE_ERROR = (uint)0x824C;
        public const uint GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR = (uint)0x824D;
        public const uint GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR = (uint)0x824E;
        public const uint GL_DEBUG_TYPE_PORTABILITY = (uint)0x824F;
        public const uint GL_DEBUG_TYPE_PERFORMANCE = (uint)0x8250;
        public const uint GL_DEBUG_TYPE_OTHER = (uint)0x8251;
        public const uint GL_MAX_DEBUG_MESSAGE_LENGTH = (uint)0x9143;
        public const uint GL_MAX_DEBUG_LOGGED_MESSAGES = (uint)0x9144;
        public const uint GL_DEBUG_LOGGED_MESSAGES = (uint)0x9145;
        public const uint GL_DEBUG_SEVERITY_HIGH = (uint)0x9146;
        public const uint GL_DEBUG_SEVERITY_MEDIUM = (uint)0x9147;
        public const uint GL_DEBUG_SEVERITY_LOW = (uint)0x9148;
        public const uint GL_DEBUG_TYPE_MARKER = (uint)0x8268;
        public const uint GL_DEBUG_TYPE_PUSH_GROUP = (uint)0x8269;
        public const uint GL_DEBUG_TYPE_POP_GROUP = (uint)0x826A;
        public const uint GL_DEBUG_SEVERITY_NOTIFICATION = (uint)0x826B;
        public const uint GL_MAX_DEBUG_GROUP_STACK_DEPTH = (uint)0x826C;
        public const uint GL_DEBUG_GROUP_STACK_DEPTH = (uint)0x826D;
        public const uint GL_BUFFER = (uint)0x82E0;
        public const uint GL_SHADER = (uint)0x82E1;
        public const uint GL_PROGRAM = (uint)0x82E2;
        public const uint GL_VERTEX_ARRAY = (uint)0x8074;
        public const uint GL_QUERY = (uint)0x82E3;
        public const uint GL_PROGRAM_PIPELINE = (uint)0x82E4;
        public const uint GL_SAMPLER = (uint)0x82E6;
        public const uint GL_MAX_LABEL_LENGTH = (uint)0x82E8;
        public const uint GL_DEBUG_OUTPUT = (uint)0x92E0;
        public const uint GL_CONTEXT_FLAG_DEBUG_BIT = (uint)0x00000002;
        public const uint GL_MAX_UNIFORM_LOCATIONS = (uint)0x826E;
        public const uint GL_FRAMEBUFFER_DEFAULT_WIDTH = (uint)0x9310;
        public const uint GL_FRAMEBUFFER_DEFAULT_HEIGHT = (uint)0x9311;
        public const uint GL_FRAMEBUFFER_DEFAULT_LAYERS = (uint)0x9312;
        public const uint GL_FRAMEBUFFER_DEFAULT_SAMPLES = (uint)0x9313;
        public const uint GL_FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS = (uint)0x9314;
        public const uint GL_MAX_FRAMEBUFFER_WIDTH = (uint)0x9315;
        public const uint GL_MAX_FRAMEBUFFER_HEIGHT = (uint)0x9316;
        public const uint GL_MAX_FRAMEBUFFER_LAYERS = (uint)0x9317;
        public const uint GL_MAX_FRAMEBUFFER_SAMPLES = (uint)0x9318;
        public const uint GL_INTERNALFORMAT_SUPPORTED = (uint)0x826F;
        public const uint GL_INTERNALFORMAT_PREFERRED = (uint)0x8270;
        public const uint GL_INTERNALFORMAT_RED_SIZE = (uint)0x8271;
        public const uint GL_INTERNALFORMAT_GREEN_SIZE = (uint)0x8272;
        public const uint GL_INTERNALFORMAT_BLUE_SIZE = (uint)0x8273;
        public const uint GL_INTERNALFORMAT_ALPHA_SIZE = (uint)0x8274;
        public const uint GL_INTERNALFORMAT_DEPTH_SIZE = (uint)0x8275;
        public const uint GL_INTERNALFORMAT_STENCIL_SIZE = (uint)0x8276;
        public const uint GL_INTERNALFORMAT_SHARED_SIZE = (uint)0x8277;
        public const uint GL_INTERNALFORMAT_RED_TYPE = (uint)0x8278;
        public const uint GL_INTERNALFORMAT_GREEN_TYPE = (uint)0x8279;
        public const uint GL_INTERNALFORMAT_BLUE_TYPE = (uint)0x827A;
        public const uint GL_INTERNALFORMAT_ALPHA_TYPE = (uint)0x827B;
        public const uint GL_INTERNALFORMAT_DEPTH_TYPE = (uint)0x827C;
        public const uint GL_INTERNALFORMAT_STENCIL_TYPE = (uint)0x827D;
        public const uint GL_MAX_WIDTH = (uint)0x827E;
        public const uint GL_MAX_HEIGHT = (uint)0x827F;
        public const uint GL_MAX_DEPTH = (uint)0x8280;
        public const uint GL_MAX_LAYERS = (uint)0x8281;
        public const uint GL_MAX_COMBINED_DIMENSIONS = (uint)0x8282;
        public const uint GL_COLOR_COMPONENTS = (uint)0x8283;
        public const uint GL_DEPTH_COMPONENTS = (uint)0x8284;
        public const uint GL_STENCIL_COMPONENTS = (uint)0x8285;
        public const uint GL_COLOR_RENDERABLE = (uint)0x8286;
        public const uint GL_DEPTH_RENDERABLE = (uint)0x8287;
        public const uint GL_STENCIL_RENDERABLE = (uint)0x8288;
        public const uint GL_FRAMEBUFFER_RENDERABLE = (uint)0x8289;
        public const uint GL_FRAMEBUFFER_RENDERABLE_LAYERED = (uint)0x828A;
        public const uint GL_FRAMEBUFFER_BLEND = (uint)0x828B;
        public const uint GL_READ_PIXELS = (uint)0x828C;
        public const uint GL_READ_PIXELS_FORMAT = (uint)0x828D;
        public const uint GL_READ_PIXELS_TYPE = (uint)0x828E;
        public const uint GL_TEXTURE_IMAGE_FORMAT = (uint)0x828F;
        public const uint GL_TEXTURE_IMAGE_TYPE = (uint)0x8290;
        public const uint GL_GET_TEXTURE_IMAGE_FORMAT = (uint)0x8291;
        public const uint GL_GET_TEXTURE_IMAGE_TYPE = (uint)0x8292;
        public const uint GL_MIPMAP = (uint)0x8293;
        public const uint GL_MANUAL_GENERATE_MIPMAP = (uint)0x8294;
        public const uint GL_AUTO_GENERATE_MIPMAP = (uint)0x8295;
        public const uint GL_COLOR_ENCODING = (uint)0x8296;
        public const uint GL_SRGB_READ = (uint)0x8297;
        public const uint GL_SRGB_WRITE = (uint)0x8298;
        public const uint GL_FILTER = (uint)0x829A;
        public const uint GL_VERTEX_TEXTURE = (uint)0x829B;
        public const uint GL_TESS_CONTROL_TEXTURE = (uint)0x829C;
        public const uint GL_TESS_EVALUATION_TEXTURE = (uint)0x829D;
        public const uint GL_GEOMETRY_TEXTURE = (uint)0x829E;
        public const uint GL_FRAGMENT_TEXTURE = (uint)0x829F;
        public const uint GL_COMPUTE_TEXTURE = (uint)0x82A0;
        public const uint GL_TEXTURE_SHADOW = (uint)0x82A1;
        public const uint GL_TEXTURE_GATHER = (uint)0x82A2;
        public const uint GL_TEXTURE_GATHER_SHADOW = (uint)0x82A3;
        public const uint GL_SHADER_IMAGE_LOAD = (uint)0x82A4;
        public const uint GL_SHADER_IMAGE_STORE = (uint)0x82A5;
        public const uint GL_SHADER_IMAGE_ATOMIC = (uint)0x82A6;
        public const uint GL_IMAGE_TEXEL_SIZE = (uint)0x82A7;
        public const uint GL_IMAGE_COMPATIBILITY_CLASS = (uint)0x82A8;
        public const uint GL_IMAGE_PIXEL_FORMAT = (uint)0x82A9;
        public const uint GL_IMAGE_PIXEL_TYPE = (uint)0x82AA;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST = (uint)0x82AC;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST = (uint)0x82AD;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE = (uint)0x82AE;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE = (uint)0x82AF;
        public const uint GL_TEXTURE_COMPRESSED_BLOCK_WIDTH = (uint)0x82B1;
        public const uint GL_TEXTURE_COMPRESSED_BLOCK_HEIGHT = (uint)0x82B2;
        public const uint GL_TEXTURE_COMPRESSED_BLOCK_SIZE = (uint)0x82B3;
        public const uint GL_CLEAR_BUFFER = (uint)0x82B4;
        public const uint GL_TEXTURE_VIEW = (uint)0x82B5;
        public const uint GL_VIEW_COMPATIBILITY_CLASS = (uint)0x82B6;
        public const uint GL_FULL_SUPPORT = (uint)0x82B7;
        public const uint GL_CAVEAT_SUPPORT = (uint)0x82B8;
        public const uint GL_IMAGE_CLASS_4_X_32 = (uint)0x82B9;
        public const uint GL_IMAGE_CLASS_2_X_32 = (uint)0x82BA;
        public const uint GL_IMAGE_CLASS_1_X_32 = (uint)0x82BB;
        public const uint GL_IMAGE_CLASS_4_X_16 = (uint)0x82BC;
        public const uint GL_IMAGE_CLASS_2_X_16 = (uint)0x82BD;
        public const uint GL_IMAGE_CLASS_1_X_16 = (uint)0x82BE;
        public const uint GL_IMAGE_CLASS_4_X_8 = (uint)0x82BF;
        public const uint GL_IMAGE_CLASS_2_X_8 = (uint)0x82C0;
        public const uint GL_IMAGE_CLASS_1_X_8 = (uint)0x82C1;
        public const uint GL_IMAGE_CLASS_11_11_10 = (uint)0x82C2;
        public const uint GL_IMAGE_CLASS_10_10_10_2 = (uint)0x82C3;
        public const uint GL_VIEW_CLASS_128_BITS = (uint)0x82C4;
        public const uint GL_VIEW_CLASS_96_BITS = (uint)0x82C5;
        public const uint GL_VIEW_CLASS_64_BITS = (uint)0x82C6;
        public const uint GL_VIEW_CLASS_48_BITS = (uint)0x82C7;
        public const uint GL_VIEW_CLASS_32_BITS = (uint)0x82C8;
        public const uint GL_VIEW_CLASS_24_BITS = (uint)0x82C9;
        public const uint GL_VIEW_CLASS_16_BITS = (uint)0x82CA;
        public const uint GL_VIEW_CLASS_8_BITS = (uint)0x82CB;
        public const uint GL_VIEW_CLASS_S3TC_DXT1_RGB = (uint)0x82CC;
        public const uint GL_VIEW_CLASS_S3TC_DXT1_RGBA = (uint)0x82CD;
        public const uint GL_VIEW_CLASS_S3TC_DXT3_RGBA = (uint)0x82CE;
        public const uint GL_VIEW_CLASS_S3TC_DXT5_RGBA = (uint)0x82CF;
        public const uint GL_VIEW_CLASS_RGTC1_RED = (uint)0x82D0;
        public const uint GL_VIEW_CLASS_RGTC2_RG = (uint)0x82D1;
        public const uint GL_VIEW_CLASS_BPTC_UNORM = (uint)0x82D2;
        public const uint GL_VIEW_CLASS_BPTC_FLOAT = (uint)0x82D3;
        public const uint GL_UNIFORM = (uint)0x92E1;
        public const uint GL_UNIFORM_BLOCK = (uint)0x92E2;
        public const uint GL_PROGRAM_INPUT = (uint)0x92E3;
        public const uint GL_PROGRAM_OUTPUT = (uint)0x92E4;
        public const uint GL_BUFFER_VARIABLE = (uint)0x92E5;
        public const uint GL_SHADER_STORAGE_BLOCK = (uint)0x92E6;
        public const uint GL_VERTEX_SUBROUTINE = (uint)0x92E8;
        public const uint GL_TESS_CONTROL_SUBROUTINE = (uint)0x92E9;
        public const uint GL_TESS_EVALUATION_SUBROUTINE = (uint)0x92EA;
        public const uint GL_GEOMETRY_SUBROUTINE = (uint)0x92EB;
        public const uint GL_FRAGMENT_SUBROUTINE = (uint)0x92EC;
        public const uint GL_COMPUTE_SUBROUTINE = (uint)0x92ED;
        public const uint GL_VERTEX_SUBROUTINE_UNIFORM = (uint)0x92EE;
        public const uint GL_TESS_CONTROL_SUBROUTINE_UNIFORM = (uint)0x92EF;
        public const uint GL_TESS_EVALUATION_SUBROUTINE_UNIFORM = (uint)0x92F0;
        public const uint GL_GEOMETRY_SUBROUTINE_UNIFORM = (uint)0x92F1;
        public const uint GL_FRAGMENT_SUBROUTINE_UNIFORM = (uint)0x92F2;
        public const uint GL_COMPUTE_SUBROUTINE_UNIFORM = (uint)0x92F3;
        public const uint GL_TRANSFORM_FEEDBACK_VARYING = (uint)0x92F4;
        public const uint GL_ACTIVE_RESOURCES = (uint)0x92F5;
        public const uint GL_MAX_NAME_LENGTH = (uint)0x92F6;
        public const uint GL_MAX_NUM_ACTIVE_VARIABLES = (uint)0x92F7;
        public const uint GL_MAX_NUM_COMPATIBLE_SUBROUTINES = (uint)0x92F8;
        public const uint GL_NAME_LENGTH = (uint)0x92F9;
        public const uint GL_TYPE = (uint)0x92FA;
        public const uint GL_ARRAY_SIZE = (uint)0x92FB;
        public const uint GL_OFFSET = (uint)0x92FC;
        public const uint GL_BLOCK_INDEX = (uint)0x92FD;
        public const uint GL_ARRAY_STRIDE = (uint)0x92FE;
        public const uint GL_MATRIX_STRIDE = (uint)0x92FF;
        public const uint GL_IS_ROW_MAJOR = (uint)0x9300;
        public const uint GL_ATOMIC_COUNTER_BUFFER_INDEX = (uint)0x9301;
        public const uint GL_BUFFER_BINDING = (uint)0x9302;
        public const uint GL_BUFFER_DATA_SIZE = (uint)0x9303;
        public const uint GL_NUM_ACTIVE_VARIABLES = (uint)0x9304;
        public const uint GL_ACTIVE_VARIABLES = (uint)0x9305;
        public const uint GL_REFERENCED_BY_VERTEX_SHADER = (uint)0x9306;
        public const uint GL_REFERENCED_BY_TESS_CONTROL_SHADER = (uint)0x9307;
        public const uint GL_REFERENCED_BY_TESS_EVALUATION_SHADER = (uint)0x9308;
        public const uint GL_REFERENCED_BY_GEOMETRY_SHADER = (uint)0x9309;
        public const uint GL_REFERENCED_BY_FRAGMENT_SHADER = (uint)0x930A;
        public const uint GL_REFERENCED_BY_COMPUTE_SHADER = (uint)0x930B;
        public const uint GL_TOP_LEVEL_ARRAY_SIZE = (uint)0x930C;
        public const uint GL_TOP_LEVEL_ARRAY_STRIDE = (uint)0x930D;
        public const uint GL_LOCATION = (uint)0x930E;
        public const uint GL_LOCATION_INDEX = (uint)0x930F;
        public const uint GL_IS_PER_PATCH = (uint)0x92E7;
        public const uint GL_SHADER_STORAGE_BUFFER = (uint)0x90D2;
        public const uint GL_SHADER_STORAGE_BUFFER_BINDING = (uint)0x90D3;
        public const uint GL_SHADER_STORAGE_BUFFER_START = (uint)0x90D4;
        public const uint GL_SHADER_STORAGE_BUFFER_SIZE = (uint)0x90D5;
        public const uint GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS = (uint)0x90D6;
        public const uint GL_MAX_GEOMETRY_SHADER_STORAGE_BLOCKS = (uint)0x90D7;
        public const uint GL_MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS = (uint)0x90D8;
        public const uint GL_MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS = (uint)0x90D9;
        public const uint GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS = (uint)0x90DA;
        public const uint GL_MAX_COMPUTE_SHADER_STORAGE_BLOCKS = (uint)0x90DB;
        public const uint GL_MAX_COMBINED_SHADER_STORAGE_BLOCKS = (uint)0x90DC;
        public const uint GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS = (uint)0x90DD;
        public const uint GL_MAX_SHADER_STORAGE_BLOCK_SIZE = (uint)0x90DE;
        public const uint GL_SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT = (uint)0x90DF;
        public const uint GL_SHADER_STORAGE_BARRIER_BIT = (uint)0x00002000;
        public const uint GL_MAX_COMBINED_SHADER_OUTPUT_RESOURCES = (uint)0x8F39;
        public const uint GL_DEPTH_STENCIL_TEXTURE_MODE = (uint)0x90EA;
        public const uint GL_TEXTURE_BUFFER_OFFSET = (uint)0x919D;
        public const uint GL_TEXTURE_BUFFER_SIZE = (uint)0x919E;
        public const uint GL_TEXTURE_BUFFER_OFFSET_ALIGNMENT = (uint)0x919F;
        public const uint GL_TEXTURE_VIEW_MIN_LEVEL = (uint)0x82DB;
        public const uint GL_TEXTURE_VIEW_NUM_LEVELS = (uint)0x82DC;
        public const uint GL_TEXTURE_VIEW_MIN_LAYER = (uint)0x82DD;
        public const uint GL_TEXTURE_VIEW_NUM_LAYERS = (uint)0x82DE;
        public const uint GL_TEXTURE_IMMUTABLE_LEVELS = (uint)0x82DF;
        public const uint GL_VERTEX_ATTRIB_BINDING = (uint)0x82D4;
        public const uint GL_VERTEX_ATTRIB_RELATIVE_OFFSET = (uint)0x82D5;
        public const uint GL_VERTEX_BINDING_DIVISOR = (uint)0x82D6;
        public const uint GL_VERTEX_BINDING_OFFSET = (uint)0x82D7;
        public const uint GL_VERTEX_BINDING_STRIDE = (uint)0x82D8;
        public const uint GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET = (uint)0x82D9;
        public const uint GL_MAX_VERTEX_ATTRIB_BINDINGS = (uint)0x82DA;
        public const uint GL_VERTEX_BINDING_BUFFER = (uint)0x8F4F;
        public const uint GL_STACK_UNDERFLOW = (uint)0x0504;
        public const uint GL_STACK_OVERFLOW = (uint)0x0503;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearBufferData(uint target, uint internalformat, uint format, uint type, IntPtr data);
        internal static glClearBufferData _glClearBufferData;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearBufferData", IsExtension=false)]
        public static void ClearBufferData(uint target, uint internalformat, uint format, uint type, IntPtr data)
        {
            _glClearBufferData(target, internalformat, format, type, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearBufferSubData(uint target, uint internalformat, IntPtr offset, IntPtr size, uint format, uint type, IntPtr data);
        internal static glClearBufferSubData _glClearBufferSubData;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearBufferSubData", IsExtension=false)]
        public static void ClearBufferSubData(uint target, uint internalformat, IntPtr offset, IntPtr size, uint format, uint type, IntPtr data)
        {
            _glClearBufferSubData(target, internalformat, offset, size, format, type, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z);
        internal static glDispatchCompute _glDispatchCompute;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDispatchCompute", IsExtension=false)]
        public static void DispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z)
        {
            _glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDispatchComputeIndirect(IntPtr indirect);
        internal static glDispatchComputeIndirect _glDispatchComputeIndirect;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDispatchComputeIndirect", IsExtension=false)]
        public static void DispatchComputeIndirect(IntPtr indirect)
        {
            _glDispatchComputeIndirect(indirect);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);
        internal static glCopyImageSubData _glCopyImageSubData;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyImageSubData", IsExtension=false)]
        public static void CopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth)
        {
            _glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferParameteri(uint target, uint pname, int param);
        internal static glFramebufferParameteri _glFramebufferParameteri;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferParameteri", IsExtension=false)]
        public static void FramebufferParameteri(uint target, uint pname, int param)
        {
            _glFramebufferParameteri(target, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetFramebufferParameteriv(uint target, uint pname, int[] @params);
        internal static glGetFramebufferParameteriv _glGetFramebufferParameteriv;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetFramebufferParameteriv", IsExtension=false)]
        public static void GetFramebufferParameteriv(uint target, uint pname, int[] @params)
        {
            _glGetFramebufferParameteriv(target, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetInternalformati64v(uint target, uint internalformat, uint pname, int bufSize, long[] @params);
        internal static glGetInternalformati64v _glGetInternalformati64v;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetInternalformati64v", IsExtension=false)]
        public static void GetInternalformati64v(uint target, uint internalformat, uint pname, int bufSize, long[] @params)
        {
            _glGetInternalformati64v(target, internalformat, pname, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth);
        internal static glInvalidateTexSubImage _glInvalidateTexSubImage;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateTexSubImage", IsExtension=false)]
        public static void InvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth)
        {
            _glInvalidateTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateTexImage(uint texture, int level);
        internal static glInvalidateTexImage _glInvalidateTexImage;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateTexImage", IsExtension=false)]
        public static void InvalidateTexImage(uint texture, int level)
        {
            _glInvalidateTexImage(texture, level);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length);
        internal static glInvalidateBufferSubData _glInvalidateBufferSubData;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateBufferSubData", IsExtension=false)]
        public static void InvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length)
        {
            _glInvalidateBufferSubData(buffer, offset, length);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateBufferData(uint buffer);
        internal static glInvalidateBufferData _glInvalidateBufferData;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateBufferData", IsExtension=false)]
        public static void InvalidateBufferData(uint buffer)
        {
            _glInvalidateBufferData(buffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateFramebuffer(uint target, int numAttachments, uint[] attachments);
        internal static glInvalidateFramebuffer _glInvalidateFramebuffer;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateFramebuffer", IsExtension=false)]
        public static void InvalidateFramebuffer(uint target, int numAttachments, uint[] attachments)
        {
            _glInvalidateFramebuffer(target, numAttachments, attachments);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateSubFramebuffer(uint target, int numAttachments, uint[] attachments, int x, int y, int width, int height);
        internal static glInvalidateSubFramebuffer _glInvalidateSubFramebuffer;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateSubFramebuffer", IsExtension=false)]
        public static void InvalidateSubFramebuffer(uint target, int numAttachments, uint[] attachments, int x, int y, int width, int height)
        {
            _glInvalidateSubFramebuffer(target, numAttachments, attachments, x, y, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMultiDrawArraysIndirect(uint mode, IntPtr indirect, int drawcount, int stride);
        internal static glMultiDrawArraysIndirect _glMultiDrawArraysIndirect;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMultiDrawArraysIndirect", IsExtension=false)]
        public static void MultiDrawArraysIndirect(uint mode, IntPtr indirect, int drawcount, int stride)
        {
            _glMultiDrawArraysIndirect(mode, indirect, drawcount, stride);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMultiDrawElementsIndirect(uint mode, uint type, IntPtr indirect, int drawcount, int stride);
        internal static glMultiDrawElementsIndirect _glMultiDrawElementsIndirect;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMultiDrawElementsIndirect", IsExtension=false)]
        public static void MultiDrawElementsIndirect(uint mode, uint type, IntPtr indirect, int drawcount, int stride)
        {
            _glMultiDrawElementsIndirect(mode, type, indirect, drawcount, stride);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, int[] @params);
        internal static glGetProgramInterfaceiv _glGetProgramInterfaceiv;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramInterfaceiv", IsExtension=false)]
        public static void GetProgramInterfaceiv(uint program, uint programInterface, uint pname, int[] @params)
        {
            _glGetProgramInterfaceiv(program, programInterface, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetProgramResourceIndex(uint program, uint programInterface, string name);
        internal static glGetProgramResourceIndex _glGetProgramResourceIndex;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramResourceIndex", IsExtension=false)]
        public static uint GetProgramResourceIndex(uint program, uint programInterface, string name)
        {
            uint data = _glGetProgramResourceIndex(program, programInterface, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, IntPtr length, byte[] name);
        internal static glGetProgramResourceName _glGetProgramResourceName;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramResourceName", IsExtension=false)]
        public static void GetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, IntPtr length, byte[] name)
        {
            _glGetProgramResourceName(program, programInterface, index, bufSize, length, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, uint[] props, int bufSize, IntPtr length, int[] @params);
        internal static glGetProgramResourceiv _glGetProgramResourceiv;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramResourceiv", IsExtension=false)]
        public static void GetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, uint[] props, int bufSize, IntPtr length, int[] @params)
        {
            _glGetProgramResourceiv(program, programInterface, index, propCount, props, bufSize, length, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate int glGetProgramResourceLocation(uint program, uint programInterface, string name);
        internal static glGetProgramResourceLocation _glGetProgramResourceLocation;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramResourceLocation", IsExtension=false)]
        public static int GetProgramResourceLocation(uint program, uint programInterface, string name)
        {
            int data = _glGetProgramResourceLocation(program, programInterface, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate int glGetProgramResourceLocationIndex(uint program, uint programInterface, string name);
        internal static glGetProgramResourceLocationIndex _glGetProgramResourceLocationIndex;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramResourceLocationIndex", IsExtension=false)]
        public static int GetProgramResourceLocationIndex(uint program, uint programInterface, string name)
        {
            int data = _glGetProgramResourceLocationIndex(program, programInterface, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding);
        internal static glShaderStorageBlockBinding _glShaderStorageBlockBinding;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glShaderStorageBlockBinding", IsExtension=false)]
        public static void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding)
        {
            _glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexBufferRange(uint target, uint internalformat, uint buffer, IntPtr offset, IntPtr size);
        internal static glTexBufferRange _glTexBufferRange;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexBufferRange", IsExtension=false)]
        public static void TexBufferRange(uint target, uint internalformat, uint buffer, IntPtr offset, IntPtr size)
        {
            _glTexBufferRange(target, internalformat, buffer, offset, size);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
        internal static glTexStorage2DMultisample _glTexStorage2DMultisample;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexStorage2DMultisample", IsExtension=false)]
        public static void TexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, bool fixedsamplelocations)
        {
            _glTexStorage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexStorage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
        internal static glTexStorage3DMultisample _glTexStorage3DMultisample;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexStorage3DMultisample", IsExtension=false)]
        public static void TexStorage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations)
        {
            _glTexStorage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureView(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);
        internal static glTextureView _glTextureView;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureView", IsExtension=false)]
        public static void TextureView(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers)
        {
            _glTextureView(texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, int stride);
        internal static glBindVertexBuffer _glBindVertexBuffer;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindVertexBuffer", IsExtension=false)]
        public static void BindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, int stride)
        {
            _glBindVertexBuffer(bindingindex, buffer, offset, stride);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribFormat(uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
        internal static glVertexAttribFormat _glVertexAttribFormat;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribFormat", IsExtension=false)]
        public static void VertexAttribFormat(uint attribindex, int size, uint type, bool normalized, uint relativeoffset)
        {
            _glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset);
        internal static glVertexAttribIFormat _glVertexAttribIFormat;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribIFormat", IsExtension=false)]
        public static void VertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset)
        {
            _glVertexAttribIFormat(attribindex, size, type, relativeoffset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset);
        internal static glVertexAttribLFormat _glVertexAttribLFormat;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribLFormat", IsExtension=false)]
        public static void VertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset)
        {
            _glVertexAttribLFormat(attribindex, size, type, relativeoffset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribBinding(uint attribindex, uint bindingindex);
        internal static glVertexAttribBinding _glVertexAttribBinding;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribBinding", IsExtension=false)]
        public static void VertexAttribBinding(uint attribindex, uint bindingindex)
        {
            _glVertexAttribBinding(attribindex, bindingindex);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexBindingDivisor(uint bindingindex, uint divisor);
        internal static glVertexBindingDivisor _glVertexBindingDivisor;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexBindingDivisor", IsExtension=false)]
        public static void VertexBindingDivisor(uint bindingindex, uint divisor)
        {
            _glVertexBindingDivisor(bindingindex, divisor);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDebugMessageControl(uint source, uint type, uint severity, int count, uint[] ids, bool enabled);
        internal static glDebugMessageControl _glDebugMessageControl;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDebugMessageControl", IsExtension=false)]
        public static void DebugMessageControl(uint source, uint type, uint severity, int count, uint[] ids, bool enabled)
        {
            _glDebugMessageControl(source, type, severity, count, ids, enabled);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, string buf);
        internal static glDebugMessageInsert _glDebugMessageInsert;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDebugMessageInsert", IsExtension=false)]
        public static void DebugMessageInsert(uint source, uint type, uint id, uint severity, int length, string buf)
        {
            _glDebugMessageInsert(source, type, id, severity, length, buf);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDebugMessageCallback(DebugProc callback, IntPtr userParam);
        internal static glDebugMessageCallback _glDebugMessageCallback;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDebugMessageCallback", IsExtension=false)]
        public static void DebugMessageCallback(DebugProc callback, IntPtr userParam)
        {
            _glDebugMessageCallback(callback, userParam);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetDebugMessageLog(uint count, int bufSize, uint[] sources, uint[] types, uint[] ids, uint[] severities, int[] lengths, byte[] messageLog);
        internal static glGetDebugMessageLog _glGetDebugMessageLog;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetDebugMessageLog", IsExtension=false)]
        public static uint GetDebugMessageLog(uint count, int bufSize, uint[] sources, uint[] types, uint[] ids, uint[] severities, int[] lengths, byte[] messageLog)
        {
            uint data = _glGetDebugMessageLog(count, bufSize, sources, types, ids, severities, lengths, messageLog);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPushDebugGroup(uint source, uint id, int length, string message);
        internal static glPushDebugGroup _glPushDebugGroup;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPushDebugGroup", IsExtension=false)]
        public static void PushDebugGroup(uint source, uint id, int length, string message)
        {
            _glPushDebugGroup(source, id, length, message);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPopDebugGroup();
        internal static glPopDebugGroup _glPopDebugGroup;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPopDebugGroup", IsExtension=false)]
        public static void PopDebugGroup()
        {
            _glPopDebugGroup();
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glObjectLabel(uint identifier, uint name, int length, string label);
        internal static glObjectLabel _glObjectLabel;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glObjectLabel", IsExtension=false)]
        public static void ObjectLabel(uint identifier, uint name, int length, string label)
        {
            _glObjectLabel(identifier, name, length, label);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetObjectLabel(uint identifier, uint name, int bufSize, IntPtr length, byte[] label);
        internal static glGetObjectLabel _glGetObjectLabel;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetObjectLabel", IsExtension=false)]
        public static void GetObjectLabel(uint identifier, uint name, int bufSize, IntPtr length, byte[] label)
        {
            _glGetObjectLabel(identifier, name, bufSize, length, label);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glObjectPtrLabel(IntPtr ptr, int length, string label);
        internal static glObjectPtrLabel _glObjectPtrLabel;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glObjectPtrLabel", IsExtension=false)]
        public static void ObjectPtrLabel(IntPtr ptr, int length, string label)
        {
            _glObjectPtrLabel(ptr, length, label);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetObjectPtrLabel(IntPtr ptr, int bufSize, IntPtr length, byte[] label);
        internal static glGetObjectPtrLabel _glGetObjectPtrLabel;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetObjectPtrLabel", IsExtension=false)]
        public static void GetObjectPtrLabel(IntPtr ptr, int bufSize, IntPtr length, byte[] label)
        {
            _glGetObjectPtrLabel(ptr, bufSize, length, label);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPointerv(uint pname, IntPtr @params);
        internal static glGetPointerv _glGetPointerv;

        [Version(Group="GL_VERSION_4_3", Version = "4.3", Profile="core", DeprecatedVersion="3.2", DeprecatedProfile="core", EntryPoint="glGetPointerv", IsExtension=false)]
        public static void GetPointerv(uint pname, IntPtr @params)
        {
            _glGetPointerv(pname, @params);
        }
        

        #endregion

        #region GL_VERSION_4_4
        public const uint GL_STENCIL_INDEX = (uint)0x1901;
        public const uint GL_UNSIGNED_INT_10F_11F_11F_REV = (uint)0x8C3B;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER = (uint)0x8C8E;
        public const uint GL_STENCIL_INDEX8 = (uint)0x8D48;
        public const uint GL_MAP_READ_BIT = (uint)0x0001;
        public const uint GL_MAP_WRITE_BIT = (uint)0x0002;
        public const uint GL_MAX_VERTEX_ATTRIB_STRIDE = (uint)0x82E5;
        public const uint GL_PRIMITIVE_RESTART_FOR_PATCHES_SUPPORTED = (uint)0x8221;
        public const uint GL_TEXTURE_BUFFER_BINDING = (uint)0x8C2A;
        public const uint GL_MAP_PERSISTENT_BIT = (uint)0x0040;
        public const uint GL_MAP_COHERENT_BIT = (uint)0x0080;
        public const uint GL_DYNAMIC_STORAGE_BIT = (uint)0x0100;
        public const uint GL_CLIENT_STORAGE_BIT = (uint)0x0200;
        public const uint GL_CLIENT_MAPPED_BUFFER_BARRIER_BIT = (uint)0x00004000;
        public const uint GL_BUFFER_IMMUTABLE_STORAGE = (uint)0x821F;
        public const uint GL_BUFFER_STORAGE_FLAGS = (uint)0x8220;
        public const uint GL_CLEAR_TEXTURE = (uint)0x9365;
        public const uint GL_LOCATION_COMPONENT = (uint)0x934A;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_INDEX = (uint)0x934B;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_STRIDE = (uint)0x934C;
        public const uint GL_QUERY_BUFFER = (uint)0x9192;
        public const uint GL_QUERY_BUFFER_BARRIER_BIT = (uint)0x00008000;
        public const uint GL_QUERY_BUFFER_BINDING = (uint)0x9193;
        public const uint GL_QUERY_RESULT_NO_WAIT = (uint)0x9194;
        public const uint GL_MIRROR_CLAMP_TO_EDGE = (uint)0x8743;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBufferStorage(uint target, IntPtr size, IntPtr data, uint flags);
        internal static glBufferStorage _glBufferStorage;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBufferStorage", IsExtension=false)]
        public static void BufferStorage(uint target, IntPtr size, IntPtr data, uint flags)
        {
            _glBufferStorage(target, size, data, flags);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearTexImage(uint texture, int level, uint format, uint type, IntPtr data);
        internal static glClearTexImage _glClearTexImage;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearTexImage", IsExtension=false)]
        public static void ClearTexImage(uint texture, int level, uint format, uint type, IntPtr data)
        {
            _glClearTexImage(texture, level, format, type, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data);
        internal static glClearTexSubImage _glClearTexSubImage;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearTexSubImage", IsExtension=false)]
        public static void ClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr data)
        {
            _glClearTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindBuffersBase(uint target, uint first, int count, uint[] buffers);
        internal static glBindBuffersBase _glBindBuffersBase;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindBuffersBase", IsExtension=false)]
        public static void BindBuffersBase(uint target, uint first, int count, uint[] buffers)
        {
            _glBindBuffersBase(target, first, count, buffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindBuffersRange(uint target, uint first, int count, uint[] buffers, IntPtr offsets, IntPtr sizes);
        internal static glBindBuffersRange _glBindBuffersRange;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindBuffersRange", IsExtension=false)]
        public static void BindBuffersRange(uint target, uint first, int count, uint[] buffers, IntPtr offsets, IntPtr sizes)
        {
            _glBindBuffersRange(target, first, count, buffers, offsets, sizes);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindTextures(uint first, int count, uint[] textures);
        internal static glBindTextures _glBindTextures;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindTextures", IsExtension=false)]
        public static void BindTextures(uint first, int count, uint[] textures)
        {
            _glBindTextures(first, count, textures);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindSamplers(uint first, int count, uint[] samplers);
        internal static glBindSamplers _glBindSamplers;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindSamplers", IsExtension=false)]
        public static void BindSamplers(uint first, int count, uint[] samplers)
        {
            _glBindSamplers(first, count, samplers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindImageTextures(uint first, int count, uint[] textures);
        internal static glBindImageTextures _glBindImageTextures;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindImageTextures", IsExtension=false)]
        public static void BindImageTextures(uint first, int count, uint[] textures)
        {
            _glBindImageTextures(first, count, textures);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindVertexBuffers(uint first, int count, uint[] buffers, IntPtr offsets, int[] strides);
        internal static glBindVertexBuffers _glBindVertexBuffers;

        [Version(Group="GL_VERSION_4_4", Version = "4.4", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindVertexBuffers", IsExtension=false)]
        public static void BindVertexBuffers(uint first, int count, uint[] buffers, IntPtr offsets, int[] strides)
        {
            _glBindVertexBuffers(first, count, buffers, offsets, strides);
        }
        

        #endregion

        #region GL_VERSION_4_5
        public const uint GL_NONE = (uint)0;
        public const uint GL_BACK = (uint)0x0405;
        public const uint GL_NO_ERROR = (uint)0;
        public const uint GL_TEXTURE_BINDING_1D = (uint)0x8068;
        public const uint GL_TEXTURE_BINDING_2D = (uint)0x8069;
        public const uint GL_TEXTURE_BINDING_3D = (uint)0x806A;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP = (uint)0x8514;
        public const uint GL_LOWER_LEFT = (uint)0x8CA1;
        public const uint GL_UPPER_LEFT = (uint)0x8CA2;
        public const uint GL_TEXTURE_BINDING_1D_ARRAY = (uint)0x8C1C;
        public const uint GL_TEXTURE_BINDING_2D_ARRAY = (uint)0x8C1D;
        public const uint GL_TEXTURE_BINDING_BUFFER = (uint)0x8C2C;
        public const uint GL_TEXTURE_BINDING_RECTANGLE = (uint)0x84F6;
        public const uint GL_TEXTURE_BINDING_2D_MULTISAMPLE = (uint)0x9104;
        public const uint GL_TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY = (uint)0x9105;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP_ARRAY = (uint)0x900A;
        public const uint GL_CONTEXT_LOST = (uint)0x0507;
        public const uint GL_NEGATIVE_ONE_TO_ONE = (uint)0x935E;
        public const uint GL_ZERO_TO_ONE = (uint)0x935F;
        public const uint GL_CLIP_ORIGIN = (uint)0x935C;
        public const uint GL_CLIP_DEPTH_MODE = (uint)0x935D;
        public const uint GL_QUERY_WAIT_INVERTED = (uint)0x8E17;
        public const uint GL_QUERY_NO_WAIT_INVERTED = (uint)0x8E18;
        public const uint GL_QUERY_BY_REGION_WAIT_INVERTED = (uint)0x8E19;
        public const uint GL_QUERY_BY_REGION_NO_WAIT_INVERTED = (uint)0x8E1A;
        public const uint GL_MAX_CULL_DISTANCES = (uint)0x82F9;
        public const uint GL_MAX_COMBINED_CLIP_AND_CULL_DISTANCES = (uint)0x82FA;
        public const uint GL_TEXTURE_TARGET = (uint)0x1006;
        public const uint GL_QUERY_TARGET = (uint)0x82EA;
        public const uint GL_GUILTY_CONTEXT_RESET = (uint)0x8253;
        public const uint GL_INNOCENT_CONTEXT_RESET = (uint)0x8254;
        public const uint GL_UNKNOWN_CONTEXT_RESET = (uint)0x8255;
        public const uint GL_RESET_NOTIFICATION_STRATEGY = (uint)0x8256;
        public const uint GL_LOSE_CONTEXT_ON_RESET = (uint)0x8252;
        public const uint GL_NO_RESET_NOTIFICATION = (uint)0x8261;
        public const uint GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT = (uint)0x00000004;
        public const uint GL_CONTEXT_RELEASE_BEHAVIOR = (uint)0x82FB;
        public const uint GL_CONTEXT_RELEASE_BEHAVIOR_FLUSH = (uint)0x82FC;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClipControl(uint origin, uint depth);
        internal static glClipControl _glClipControl;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClipControl", IsExtension=false)]
        public static void ClipControl(uint origin, uint depth)
        {
            _glClipControl(origin, depth);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateTransformFeedbacks(int n, IntPtr ids);
        internal static glCreateTransformFeedbacks _glCreateTransformFeedbacks;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateTransformFeedbacks", IsExtension=false)]
        public static void CreateTransformFeedbacks(int n, IntPtr ids)
        {
            _glCreateTransformFeedbacks(n, ids);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTransformFeedbackBufferBase(uint xfb, uint index, uint buffer);
        internal static glTransformFeedbackBufferBase _glTransformFeedbackBufferBase;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTransformFeedbackBufferBase", IsExtension=false)]
        public static void TransformFeedbackBufferBase(uint xfb, uint index, uint buffer)
        {
            _glTransformFeedbackBufferBase(xfb, index, buffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTransformFeedbackBufferRange(uint xfb, uint index, uint buffer, IntPtr offset, IntPtr size);
        internal static glTransformFeedbackBufferRange _glTransformFeedbackBufferRange;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTransformFeedbackBufferRange", IsExtension=false)]
        public static void TransformFeedbackBufferRange(uint xfb, uint index, uint buffer, IntPtr offset, IntPtr size)
        {
            _glTransformFeedbackBufferRange(xfb, index, buffer, offset, size);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTransformFeedbackiv(uint xfb, uint pname, IntPtr param);
        internal static glGetTransformFeedbackiv _glGetTransformFeedbackiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTransformFeedbackiv", IsExtension=false)]
        public static void GetTransformFeedbackiv(uint xfb, uint pname, IntPtr param)
        {
            _glGetTransformFeedbackiv(xfb, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTransformFeedbacki_v(uint xfb, uint pname, uint index, IntPtr param);
        internal static glGetTransformFeedbacki_v _glGetTransformFeedbacki_v;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTransformFeedbacki_v", IsExtension=false)]
        public static void GetTransformFeedbacki_v(uint xfb, uint pname, uint index, IntPtr param)
        {
            _glGetTransformFeedbacki_v(xfb, pname, index, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTransformFeedbacki64_v(uint xfb, uint pname, uint index, IntPtr param);
        internal static glGetTransformFeedbacki64_v _glGetTransformFeedbacki64_v;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTransformFeedbacki64_v", IsExtension=false)]
        public static void GetTransformFeedbacki64_v(uint xfb, uint pname, uint index, IntPtr param)
        {
            _glGetTransformFeedbacki64_v(xfb, pname, index, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateBuffers(int n, IntPtr buffers);
        internal static glCreateBuffers _glCreateBuffers;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateBuffers", IsExtension=false)]
        public static void CreateBuffers(int n, IntPtr buffers)
        {
            _glCreateBuffers(n, buffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedBufferStorage(uint buffer, IntPtr size, IntPtr data, uint flags);
        internal static glNamedBufferStorage _glNamedBufferStorage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedBufferStorage", IsExtension=false)]
        public static void NamedBufferStorage(uint buffer, IntPtr size, IntPtr data, uint flags)
        {
            _glNamedBufferStorage(buffer, size, data, flags);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedBufferData(uint buffer, IntPtr size, IntPtr data, uint usage);
        internal static glNamedBufferData _glNamedBufferData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedBufferData", IsExtension=false)]
        public static void NamedBufferData(uint buffer, IntPtr size, IntPtr data, uint usage)
        {
            _glNamedBufferData(buffer, size, data, usage);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedBufferSubData(uint buffer, IntPtr offset, IntPtr size, IntPtr data);
        internal static glNamedBufferSubData _glNamedBufferSubData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedBufferSubData", IsExtension=false)]
        public static void NamedBufferSubData(uint buffer, IntPtr offset, IntPtr size, IntPtr data)
        {
            _glNamedBufferSubData(buffer, offset, size, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyNamedBufferSubData(uint readBuffer, uint writeBuffer, IntPtr readOffset, IntPtr writeOffset, IntPtr size);
        internal static glCopyNamedBufferSubData _glCopyNamedBufferSubData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyNamedBufferSubData", IsExtension=false)]
        public static void CopyNamedBufferSubData(uint readBuffer, uint writeBuffer, IntPtr readOffset, IntPtr writeOffset, IntPtr size)
        {
            _glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearNamedBufferData(uint buffer, uint internalformat, uint format, uint type, IntPtr data);
        internal static glClearNamedBufferData _glClearNamedBufferData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearNamedBufferData", IsExtension=false)]
        public static void ClearNamedBufferData(uint buffer, uint internalformat, uint format, uint type, IntPtr data)
        {
            _glClearNamedBufferData(buffer, internalformat, format, type, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearNamedBufferSubData(uint buffer, uint internalformat, IntPtr offset, IntPtr size, uint format, uint type, IntPtr data);
        internal static glClearNamedBufferSubData _glClearNamedBufferSubData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearNamedBufferSubData", IsExtension=false)]
        public static void ClearNamedBufferSubData(uint buffer, uint internalformat, IntPtr offset, IntPtr size, uint format, uint type, IntPtr data)
        {
            _glClearNamedBufferSubData(buffer, internalformat, offset, size, format, type, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glMapNamedBuffer(uint buffer, uint access);
        internal static glMapNamedBuffer _glMapNamedBuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMapNamedBuffer", IsExtension=false)]
        public static IntPtr MapNamedBuffer(uint buffer, uint access)
        {
            IntPtr data = _glMapNamedBuffer(buffer, access);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glMapNamedBufferRange(uint buffer, IntPtr offset, IntPtr length, uint access);
        internal static glMapNamedBufferRange _glMapNamedBufferRange;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMapNamedBufferRange", IsExtension=false)]
        public static IntPtr MapNamedBufferRange(uint buffer, IntPtr offset, IntPtr length, uint access)
        {
            IntPtr data = _glMapNamedBufferRange(buffer, offset, length, access);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glUnmapNamedBuffer(uint buffer);
        internal static glUnmapNamedBuffer _glUnmapNamedBuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUnmapNamedBuffer", IsExtension=false)]
        public static bool UnmapNamedBuffer(uint buffer)
        {
            bool data = _glUnmapNamedBuffer(buffer);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFlushMappedNamedBufferRange(uint buffer, IntPtr offset, IntPtr length);
        internal static glFlushMappedNamedBufferRange _glFlushMappedNamedBufferRange;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFlushMappedNamedBufferRange", IsExtension=false)]
        public static void FlushMappedNamedBufferRange(uint buffer, IntPtr offset, IntPtr length)
        {
            _glFlushMappedNamedBufferRange(buffer, offset, length);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedBufferParameteriv(uint buffer, uint pname, IntPtr @params);
        internal static glGetNamedBufferParameteriv _glGetNamedBufferParameteriv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedBufferParameteriv", IsExtension=false)]
        public static void GetNamedBufferParameteriv(uint buffer, uint pname, IntPtr @params)
        {
            _glGetNamedBufferParameteriv(buffer, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedBufferParameteri64v(uint buffer, uint pname, IntPtr @params);
        internal static glGetNamedBufferParameteri64v _glGetNamedBufferParameteri64v;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedBufferParameteri64v", IsExtension=false)]
        public static void GetNamedBufferParameteri64v(uint buffer, uint pname, IntPtr @params)
        {
            _glGetNamedBufferParameteri64v(buffer, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedBufferPointerv(uint buffer, uint pname, IntPtr @params);
        internal static glGetNamedBufferPointerv _glGetNamedBufferPointerv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedBufferPointerv", IsExtension=false)]
        public static void GetNamedBufferPointerv(uint buffer, uint pname, IntPtr @params)
        {
            _glGetNamedBufferPointerv(buffer, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedBufferSubData(uint buffer, IntPtr offset, IntPtr size, IntPtr data);
        internal static glGetNamedBufferSubData _glGetNamedBufferSubData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedBufferSubData", IsExtension=false)]
        public static void GetNamedBufferSubData(uint buffer, IntPtr offset, IntPtr size, IntPtr data)
        {
            _glGetNamedBufferSubData(buffer, offset, size, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateFramebuffers(int n, IntPtr framebuffers);
        internal static glCreateFramebuffers _glCreateFramebuffers;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateFramebuffers", IsExtension=false)]
        public static void CreateFramebuffers(int n, IntPtr framebuffers)
        {
            _glCreateFramebuffers(n, framebuffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferRenderbuffer(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer);
        internal static glNamedFramebufferRenderbuffer _glNamedFramebufferRenderbuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferRenderbuffer", IsExtension=false)]
        public static void NamedFramebufferRenderbuffer(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer)
        {
            _glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferParameteri(uint framebuffer, uint pname, int param);
        internal static glNamedFramebufferParameteri _glNamedFramebufferParameteri;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferParameteri", IsExtension=false)]
        public static void NamedFramebufferParameteri(uint framebuffer, uint pname, int param)
        {
            _glNamedFramebufferParameteri(framebuffer, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferTexture(uint framebuffer, uint attachment, uint texture, int level);
        internal static glNamedFramebufferTexture _glNamedFramebufferTexture;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferTexture", IsExtension=false)]
        public static void NamedFramebufferTexture(uint framebuffer, uint attachment, uint texture, int level)
        {
            _glNamedFramebufferTexture(framebuffer, attachment, texture, level);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferTextureLayer(uint framebuffer, uint attachment, uint texture, int level, int layer);
        internal static glNamedFramebufferTextureLayer _glNamedFramebufferTextureLayer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferTextureLayer", IsExtension=false)]
        public static void NamedFramebufferTextureLayer(uint framebuffer, uint attachment, uint texture, int level, int layer)
        {
            _glNamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferDrawBuffer(uint framebuffer, uint buf);
        internal static glNamedFramebufferDrawBuffer _glNamedFramebufferDrawBuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferDrawBuffer", IsExtension=false)]
        public static void NamedFramebufferDrawBuffer(uint framebuffer, uint buf)
        {
            _glNamedFramebufferDrawBuffer(framebuffer, buf);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferDrawBuffers(uint framebuffer, int n, IntPtr bufs);
        internal static glNamedFramebufferDrawBuffers _glNamedFramebufferDrawBuffers;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferDrawBuffers", IsExtension=false)]
        public static void NamedFramebufferDrawBuffers(uint framebuffer, int n, IntPtr bufs)
        {
            _glNamedFramebufferDrawBuffers(framebuffer, n, bufs);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferReadBuffer(uint framebuffer, uint src);
        internal static glNamedFramebufferReadBuffer _glNamedFramebufferReadBuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferReadBuffer", IsExtension=false)]
        public static void NamedFramebufferReadBuffer(uint framebuffer, uint src)
        {
            _glNamedFramebufferReadBuffer(framebuffer, src);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateNamedFramebufferData(uint framebuffer, int numAttachments, IntPtr attachments);
        internal static glInvalidateNamedFramebufferData _glInvalidateNamedFramebufferData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateNamedFramebufferData", IsExtension=false)]
        public static void InvalidateNamedFramebufferData(uint framebuffer, int numAttachments, IntPtr attachments)
        {
            _glInvalidateNamedFramebufferData(framebuffer, numAttachments, attachments);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, IntPtr attachments, int x, int y, int width, int height);
        internal static glInvalidateNamedFramebufferSubData _glInvalidateNamedFramebufferSubData;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInvalidateNamedFramebufferSubData", IsExtension=false)]
        public static void InvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, IntPtr attachments, int x, int y, int width, int height)
        {
            _glInvalidateNamedFramebufferSubData(framebuffer, numAttachments, attachments, x, y, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearNamedFramebufferiv(uint framebuffer, uint buffer, int drawbuffer, IntPtr value);
        internal static glClearNamedFramebufferiv _glClearNamedFramebufferiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearNamedFramebufferiv", IsExtension=false)]
        public static void ClearNamedFramebufferiv(uint framebuffer, uint buffer, int drawbuffer, IntPtr value)
        {
            _glClearNamedFramebufferiv(framebuffer, buffer, drawbuffer, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearNamedFramebufferuiv(uint framebuffer, uint buffer, int drawbuffer, IntPtr value);
        internal static glClearNamedFramebufferuiv _glClearNamedFramebufferuiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearNamedFramebufferuiv", IsExtension=false)]
        public static void ClearNamedFramebufferuiv(uint framebuffer, uint buffer, int drawbuffer, IntPtr value)
        {
            _glClearNamedFramebufferuiv(framebuffer, buffer, drawbuffer, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearNamedFramebufferfv(uint framebuffer, uint buffer, int drawbuffer, IntPtr value);
        internal static glClearNamedFramebufferfv _glClearNamedFramebufferfv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearNamedFramebufferfv", IsExtension=false)]
        public static void ClearNamedFramebufferfv(uint framebuffer, uint buffer, int drawbuffer, IntPtr value)
        {
            _glClearNamedFramebufferfv(framebuffer, buffer, drawbuffer, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glClearNamedFramebufferfi(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil);
        internal static glClearNamedFramebufferfi _glClearNamedFramebufferfi;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glClearNamedFramebufferfi", IsExtension=false)]
        public static void ClearNamedFramebufferfi(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil)
        {
            _glClearNamedFramebufferfi(framebuffer, buffer, drawbuffer, depth, stencil);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
        internal static glBlitNamedFramebuffer _glBlitNamedFramebuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlitNamedFramebuffer", IsExtension=false)]
        public static void BlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter)
        {
            _glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glCheckNamedFramebufferStatus(uint framebuffer, uint target);
        internal static glCheckNamedFramebufferStatus _glCheckNamedFramebufferStatus;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCheckNamedFramebufferStatus", IsExtension=false)]
        public static uint CheckNamedFramebufferStatus(uint framebuffer, uint target)
        {
            uint data = _glCheckNamedFramebufferStatus(framebuffer, target);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedFramebufferParameteriv(uint framebuffer, uint pname, IntPtr param);
        internal static glGetNamedFramebufferParameteriv _glGetNamedFramebufferParameteriv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedFramebufferParameteriv", IsExtension=false)]
        public static void GetNamedFramebufferParameteriv(uint framebuffer, uint pname, IntPtr param)
        {
            _glGetNamedFramebufferParameteriv(framebuffer, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedFramebufferAttachmentParameteriv(uint framebuffer, uint attachment, uint pname, IntPtr @params);
        internal static glGetNamedFramebufferAttachmentParameteriv _glGetNamedFramebufferAttachmentParameteriv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedFramebufferAttachmentParameteriv", IsExtension=false)]
        public static void GetNamedFramebufferAttachmentParameteriv(uint framebuffer, uint attachment, uint pname, IntPtr @params)
        {
            _glGetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateRenderbuffers(int n, IntPtr renderbuffers);
        internal static glCreateRenderbuffers _glCreateRenderbuffers;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateRenderbuffers", IsExtension=false)]
        public static void CreateRenderbuffers(int n, IntPtr renderbuffers)
        {
            _glCreateRenderbuffers(n, renderbuffers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedRenderbufferStorage(uint renderbuffer, uint internalformat, int width, int height);
        internal static glNamedRenderbufferStorage _glNamedRenderbufferStorage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedRenderbufferStorage", IsExtension=false)]
        public static void NamedRenderbufferStorage(uint renderbuffer, uint internalformat, int width, int height)
        {
            _glNamedRenderbufferStorage(renderbuffer, internalformat, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedRenderbufferStorageMultisample(uint renderbuffer, int samples, uint internalformat, int width, int height);
        internal static glNamedRenderbufferStorageMultisample _glNamedRenderbufferStorageMultisample;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedRenderbufferStorageMultisample", IsExtension=false)]
        public static void NamedRenderbufferStorageMultisample(uint renderbuffer, int samples, uint internalformat, int width, int height)
        {
            _glNamedRenderbufferStorageMultisample(renderbuffer, samples, internalformat, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedRenderbufferParameteriv(uint renderbuffer, uint pname, IntPtr @params);
        internal static glGetNamedRenderbufferParameteriv _glGetNamedRenderbufferParameteriv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedRenderbufferParameteriv", IsExtension=false)]
        public static void GetNamedRenderbufferParameteriv(uint renderbuffer, uint pname, IntPtr @params)
        {
            _glGetNamedRenderbufferParameteriv(renderbuffer, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateTextures(uint target, int n, IntPtr textures);
        internal static glCreateTextures _glCreateTextures;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateTextures", IsExtension=false)]
        public static void CreateTextures(uint target, int n, IntPtr textures)
        {
            _glCreateTextures(target, n, textures);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureBuffer(uint texture, uint internalformat, uint buffer);
        internal static glTextureBuffer _glTextureBuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureBuffer", IsExtension=false)]
        public static void TextureBuffer(uint texture, uint internalformat, uint buffer)
        {
            _glTextureBuffer(texture, internalformat, buffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureBufferRange(uint texture, uint internalformat, uint buffer, IntPtr offset, IntPtr size);
        internal static glTextureBufferRange _glTextureBufferRange;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureBufferRange", IsExtension=false)]
        public static void TextureBufferRange(uint texture, uint internalformat, uint buffer, IntPtr offset, IntPtr size)
        {
            _glTextureBufferRange(texture, internalformat, buffer, offset, size);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureStorage1D(uint texture, int levels, uint internalformat, int width);
        internal static glTextureStorage1D _glTextureStorage1D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureStorage1D", IsExtension=false)]
        public static void TextureStorage1D(uint texture, int levels, uint internalformat, int width)
        {
            _glTextureStorage1D(texture, levels, internalformat, width);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureStorage2D(uint texture, int levels, uint internalformat, int width, int height);
        internal static glTextureStorage2D _glTextureStorage2D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureStorage2D", IsExtension=false)]
        public static void TextureStorage2D(uint texture, int levels, uint internalformat, int width, int height)
        {
            _glTextureStorage2D(texture, levels, internalformat, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureStorage3D(uint texture, int levels, uint internalformat, int width, int height, int depth);
        internal static glTextureStorage3D _glTextureStorage3D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureStorage3D", IsExtension=false)]
        public static void TextureStorage3D(uint texture, int levels, uint internalformat, int width, int height, int depth)
        {
            _glTextureStorage3D(texture, levels, internalformat, width, height, depth);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureStorage2DMultisample(uint texture, int samples, uint internalformat, int width, int height, bool fixedsamplelocations);
        internal static glTextureStorage2DMultisample _glTextureStorage2DMultisample;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureStorage2DMultisample", IsExtension=false)]
        public static void TextureStorage2DMultisample(uint texture, int samples, uint internalformat, int width, int height, bool fixedsamplelocations)
        {
            _glTextureStorage2DMultisample(texture, samples, internalformat, width, height, fixedsamplelocations);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureStorage3DMultisample(uint texture, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations);
        internal static glTextureStorage3DMultisample _glTextureStorage3DMultisample;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureStorage3DMultisample", IsExtension=false)]
        public static void TextureStorage3DMultisample(uint texture, int samples, uint internalformat, int width, int height, int depth, bool fixedsamplelocations)
        {
            _glTextureStorage3DMultisample(texture, samples, internalformat, width, height, depth, fixedsamplelocations);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
        internal static glTextureSubImage1D _glTextureSubImage1D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureSubImage1D", IsExtension=false)]
        public static void TextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, uint type, IntPtr pixels)
        {
            _glTextureSubImage1D(texture, level, xoffset, width, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
        internal static glTextureSubImage2D _glTextureSubImage2D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureSubImage2D", IsExtension=false)]
        public static void TextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels)
        {
            _glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
        internal static glTextureSubImage3D _glTextureSubImage3D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureSubImage3D", IsExtension=false)]
        public static void TextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels)
        {
            _glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
        internal static glCompressedTextureSubImage1D _glCompressedTextureSubImage1D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTextureSubImage1D", IsExtension=false)]
        public static void CompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, int imageSize, IntPtr data)
        {
            _glCompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
        internal static glCompressedTextureSubImage2D _glCompressedTextureSubImage2D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTextureSubImage2D", IsExtension=false)]
        public static void CompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data)
        {
            _glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
        internal static glCompressedTextureSubImage3D _glCompressedTextureSubImage3D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompressedTextureSubImage3D", IsExtension=false)]
        public static void CompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data)
        {
            _glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width);
        internal static glCopyTextureSubImage1D _glCopyTextureSubImage1D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTextureSubImage1D", IsExtension=false)]
        public static void CopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width)
        {
            _glCopyTextureSubImage1D(texture, level, xoffset, x, y, width);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height);
        internal static glCopyTextureSubImage2D _glCopyTextureSubImage2D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTextureSubImage2D", IsExtension=false)]
        public static void CopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height)
        {
            _glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
        internal static glCopyTextureSubImage3D _glCopyTextureSubImage3D;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyTextureSubImage3D", IsExtension=false)]
        public static void CopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height)
        {
            _glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureParameterf(uint texture, uint pname, float param);
        internal static glTextureParameterf _glTextureParameterf;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureParameterf", IsExtension=false)]
        public static void TextureParameterf(uint texture, uint pname, float param)
        {
            _glTextureParameterf(texture, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureParameterfv(uint texture, uint pname, IntPtr param);
        internal static glTextureParameterfv _glTextureParameterfv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureParameterfv", IsExtension=false)]
        public static void TextureParameterfv(uint texture, uint pname, IntPtr param)
        {
            _glTextureParameterfv(texture, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureParameteri(uint texture, uint pname, int param);
        internal static glTextureParameteri _glTextureParameteri;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureParameteri", IsExtension=false)]
        public static void TextureParameteri(uint texture, uint pname, int param)
        {
            _glTextureParameteri(texture, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureParameterIiv(uint texture, uint pname, IntPtr @params);
        internal static glTextureParameterIiv _glTextureParameterIiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureParameterIiv", IsExtension=false)]
        public static void TextureParameterIiv(uint texture, uint pname, IntPtr @params)
        {
            _glTextureParameterIiv(texture, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureParameterIuiv(uint texture, uint pname, IntPtr @params);
        internal static glTextureParameterIuiv _glTextureParameterIuiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureParameterIuiv", IsExtension=false)]
        public static void TextureParameterIuiv(uint texture, uint pname, IntPtr @params)
        {
            _glTextureParameterIuiv(texture, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureParameteriv(uint texture, uint pname, IntPtr param);
        internal static glTextureParameteriv _glTextureParameteriv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureParameteriv", IsExtension=false)]
        public static void TextureParameteriv(uint texture, uint pname, IntPtr param)
        {
            _glTextureParameteriv(texture, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenerateTextureMipmap(uint texture);
        internal static glGenerateTextureMipmap _glGenerateTextureMipmap;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenerateTextureMipmap", IsExtension=false)]
        public static void GenerateTextureMipmap(uint texture)
        {
            _glGenerateTextureMipmap(texture);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBindTextureUnit(uint unit, uint texture);
        internal static glBindTextureUnit _glBindTextureUnit;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBindTextureUnit", IsExtension=false)]
        public static void BindTextureUnit(uint unit, uint texture)
        {
            _glBindTextureUnit(unit, texture);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureImage(uint texture, int level, uint format, uint type, int bufSize, IntPtr pixels);
        internal static glGetTextureImage _glGetTextureImage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureImage", IsExtension=false)]
        public static void GetTextureImage(uint texture, int level, uint format, uint type, int bufSize, IntPtr pixels)
        {
            _glGetTextureImage(texture, level, format, type, bufSize, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels);
        internal static glGetCompressedTextureImage _glGetCompressedTextureImage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetCompressedTextureImage", IsExtension=false)]
        public static void GetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels)
        {
            _glGetCompressedTextureImage(texture, level, bufSize, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureLevelParameterfv(uint texture, int level, uint pname, IntPtr @params);
        internal static glGetTextureLevelParameterfv _glGetTextureLevelParameterfv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureLevelParameterfv", IsExtension=false)]
        public static void GetTextureLevelParameterfv(uint texture, int level, uint pname, IntPtr @params)
        {
            _glGetTextureLevelParameterfv(texture, level, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureLevelParameteriv(uint texture, int level, uint pname, IntPtr @params);
        internal static glGetTextureLevelParameteriv _glGetTextureLevelParameteriv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureLevelParameteriv", IsExtension=false)]
        public static void GetTextureLevelParameteriv(uint texture, int level, uint pname, IntPtr @params)
        {
            _glGetTextureLevelParameteriv(texture, level, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureParameterfv(uint texture, uint pname, IntPtr @params);
        internal static glGetTextureParameterfv _glGetTextureParameterfv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureParameterfv", IsExtension=false)]
        public static void GetTextureParameterfv(uint texture, uint pname, IntPtr @params)
        {
            _glGetTextureParameterfv(texture, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureParameterIiv(uint texture, uint pname, IntPtr @params);
        internal static glGetTextureParameterIiv _glGetTextureParameterIiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureParameterIiv", IsExtension=false)]
        public static void GetTextureParameterIiv(uint texture, uint pname, IntPtr @params)
        {
            _glGetTextureParameterIiv(texture, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureParameterIuiv(uint texture, uint pname, IntPtr @params);
        internal static glGetTextureParameterIuiv _glGetTextureParameterIuiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureParameterIuiv", IsExtension=false)]
        public static void GetTextureParameterIuiv(uint texture, uint pname, IntPtr @params)
        {
            _glGetTextureParameterIuiv(texture, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureParameteriv(uint texture, uint pname, IntPtr @params);
        internal static glGetTextureParameteriv _glGetTextureParameteriv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureParameteriv", IsExtension=false)]
        public static void GetTextureParameteriv(uint texture, uint pname, IntPtr @params)
        {
            _glGetTextureParameteriv(texture, pname, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateVertexArrays(int n, IntPtr arrays);
        internal static glCreateVertexArrays _glCreateVertexArrays;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateVertexArrays", IsExtension=false)]
        public static void CreateVertexArrays(int n, IntPtr arrays)
        {
            _glCreateVertexArrays(n, arrays);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDisableVertexArrayAttrib(uint vaobj, uint index);
        internal static glDisableVertexArrayAttrib _glDisableVertexArrayAttrib;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDisableVertexArrayAttrib", IsExtension=false)]
        public static void DisableVertexArrayAttrib(uint vaobj, uint index)
        {
            _glDisableVertexArrayAttrib(vaobj, index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEnableVertexArrayAttrib(uint vaobj, uint index);
        internal static glEnableVertexArrayAttrib _glEnableVertexArrayAttrib;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEnableVertexArrayAttrib", IsExtension=false)]
        public static void EnableVertexArrayAttrib(uint vaobj, uint index)
        {
            _glEnableVertexArrayAttrib(vaobj, index);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayElementBuffer(uint vaobj, uint buffer);
        internal static glVertexArrayElementBuffer _glVertexArrayElementBuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayElementBuffer", IsExtension=false)]
        public static void VertexArrayElementBuffer(uint vaobj, uint buffer)
        {
            _glVertexArrayElementBuffer(vaobj, buffer);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride);
        internal static glVertexArrayVertexBuffer _glVertexArrayVertexBuffer;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayVertexBuffer", IsExtension=false)]
        public static void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride)
        {
            _glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayVertexBuffers(uint vaobj, uint first, int count, IntPtr buffers, IntPtr offsets, IntPtr strides);
        internal static glVertexArrayVertexBuffers _glVertexArrayVertexBuffers;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayVertexBuffers", IsExtension=false)]
        public static void VertexArrayVertexBuffers(uint vaobj, uint first, int count, IntPtr buffers, IntPtr offsets, IntPtr strides)
        {
            _glVertexArrayVertexBuffers(vaobj, first, count, buffers, offsets, strides);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex);
        internal static glVertexArrayAttribBinding _glVertexArrayAttribBinding;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayAttribBinding", IsExtension=false)]
        public static void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex)
        {
            _glVertexArrayAttribBinding(vaobj, attribindex, bindingindex);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
        internal static glVertexArrayAttribFormat _glVertexArrayAttribFormat;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayAttribFormat", IsExtension=false)]
        public static void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset)
        {
            _glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativeoffset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
        internal static glVertexArrayAttribIFormat _glVertexArrayAttribIFormat;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayAttribIFormat", IsExtension=false)]
        public static void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset)
        {
            _glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativeoffset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
        internal static glVertexArrayAttribLFormat _glVertexArrayAttribLFormat;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayAttribLFormat", IsExtension=false)]
        public static void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset)
        {
            _glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativeoffset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor);
        internal static glVertexArrayBindingDivisor _glVertexArrayBindingDivisor;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexArrayBindingDivisor", IsExtension=false)]
        public static void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor)
        {
            _glVertexArrayBindingDivisor(vaobj, bindingindex, divisor);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexArrayiv(uint vaobj, uint pname, IntPtr param);
        internal static glGetVertexArrayiv _glGetVertexArrayiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexArrayiv", IsExtension=false)]
        public static void GetVertexArrayiv(uint vaobj, uint pname, IntPtr param)
        {
            _glGetVertexArrayiv(vaobj, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexArrayIndexediv(uint vaobj, uint index, uint pname, IntPtr param);
        internal static glGetVertexArrayIndexediv _glGetVertexArrayIndexediv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexArrayIndexediv", IsExtension=false)]
        public static void GetVertexArrayIndexediv(uint vaobj, uint index, uint pname, IntPtr param)
        {
            _glGetVertexArrayIndexediv(vaobj, index, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexArrayIndexed64iv(uint vaobj, uint index, uint pname, IntPtr param);
        internal static glGetVertexArrayIndexed64iv _glGetVertexArrayIndexed64iv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexArrayIndexed64iv", IsExtension=false)]
        public static void GetVertexArrayIndexed64iv(uint vaobj, uint index, uint pname, IntPtr param)
        {
            _glGetVertexArrayIndexed64iv(vaobj, index, pname, param);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateSamplers(int n, IntPtr samplers);
        internal static glCreateSamplers _glCreateSamplers;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateSamplers", IsExtension=false)]
        public static void CreateSamplers(int n, IntPtr samplers)
        {
            _glCreateSamplers(n, samplers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateProgramPipelines(int n, IntPtr pipelines);
        internal static glCreateProgramPipelines _glCreateProgramPipelines;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateProgramPipelines", IsExtension=false)]
        public static void CreateProgramPipelines(int n, IntPtr pipelines)
        {
            _glCreateProgramPipelines(n, pipelines);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreateQueries(uint target, int n, IntPtr ids);
        internal static glCreateQueries _glCreateQueries;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateQueries", IsExtension=false)]
        public static void CreateQueries(uint target, int n, IntPtr ids)
        {
            _glCreateQueries(target, n, ids);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryBufferObjecti64v(uint id, uint buffer, uint pname, IntPtr offset);
        internal static glGetQueryBufferObjecti64v _glGetQueryBufferObjecti64v;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryBufferObjecti64v", IsExtension=false)]
        public static void GetQueryBufferObjecti64v(uint id, uint buffer, uint pname, IntPtr offset)
        {
            _glGetQueryBufferObjecti64v(id, buffer, pname, offset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryBufferObjectiv(uint id, uint buffer, uint pname, IntPtr offset);
        internal static glGetQueryBufferObjectiv _glGetQueryBufferObjectiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryBufferObjectiv", IsExtension=false)]
        public static void GetQueryBufferObjectiv(uint id, uint buffer, uint pname, IntPtr offset)
        {
            _glGetQueryBufferObjectiv(id, buffer, pname, offset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryBufferObjectui64v(uint id, uint buffer, uint pname, IntPtr offset);
        internal static glGetQueryBufferObjectui64v _glGetQueryBufferObjectui64v;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryBufferObjectui64v", IsExtension=false)]
        public static void GetQueryBufferObjectui64v(uint id, uint buffer, uint pname, IntPtr offset)
        {
            _glGetQueryBufferObjectui64v(id, buffer, pname, offset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetQueryBufferObjectuiv(uint id, uint buffer, uint pname, IntPtr offset);
        internal static glGetQueryBufferObjectuiv _glGetQueryBufferObjectuiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetQueryBufferObjectuiv", IsExtension=false)]
        public static void GetQueryBufferObjectuiv(uint id, uint buffer, uint pname, IntPtr offset)
        {
            _glGetQueryBufferObjectuiv(id, buffer, pname, offset);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMemoryBarrierByRegion(uint barriers);
        internal static glMemoryBarrierByRegion _glMemoryBarrierByRegion;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMemoryBarrierByRegion", IsExtension=false)]
        public static void MemoryBarrierByRegion(uint barriers)
        {
            _glMemoryBarrierByRegion(barriers);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, IntPtr pixels);
        internal static glGetTextureSubImage _glGetTextureSubImage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureSubImage", IsExtension=false)]
        public static void GetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, IntPtr pixels)
        {
            _glGetTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels);
        internal static glGetCompressedTextureSubImage _glGetCompressedTextureSubImage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetCompressedTextureSubImage", IsExtension=false)]
        public static void GetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels)
        {
            _glGetCompressedTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetGraphicsResetStatus();
        internal static glGetGraphicsResetStatus _glGetGraphicsResetStatus;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetGraphicsResetStatus", IsExtension=false)]
        public static uint GetGraphicsResetStatus()
        {
            uint data = _glGetGraphicsResetStatus();
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnCompressedTexImage(uint target, int lod, int bufSize, IntPtr pixels);
        internal static glGetnCompressedTexImage _glGetnCompressedTexImage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnCompressedTexImage", IsExtension=false)]
        public static void GetnCompressedTexImage(uint target, int lod, int bufSize, IntPtr pixels)
        {
            _glGetnCompressedTexImage(target, lod, bufSize, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnTexImage(uint target, int level, uint format, uint type, int bufSize, IntPtr pixels);
        internal static glGetnTexImage _glGetnTexImage;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnTexImage", IsExtension=false)]
        public static void GetnTexImage(uint target, int level, uint format, uint type, int bufSize, IntPtr pixels)
        {
            _glGetnTexImage(target, level, format, type, bufSize, pixels);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformdv(uint program, int location, int bufSize, IntPtr @params);
        internal static glGetnUniformdv _glGetnUniformdv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformdv", IsExtension=false)]
        public static void GetnUniformdv(uint program, int location, int bufSize, IntPtr @params)
        {
            _glGetnUniformdv(program, location, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformfv(uint program, int location, int bufSize, IntPtr @params);
        internal static glGetnUniformfv _glGetnUniformfv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformfv", IsExtension=false)]
        public static void GetnUniformfv(uint program, int location, int bufSize, IntPtr @params)
        {
            _glGetnUniformfv(program, location, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformiv(uint program, int location, int bufSize, IntPtr @params);
        internal static glGetnUniformiv _glGetnUniformiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformiv", IsExtension=false)]
        public static void GetnUniformiv(uint program, int location, int bufSize, IntPtr @params)
        {
            _glGetnUniformiv(program, location, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformuiv(uint program, int location, int bufSize, IntPtr @params);
        internal static glGetnUniformuiv _glGetnUniformuiv;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformuiv", IsExtension=false)]
        public static void GetnUniformuiv(uint program, int location, int bufSize, IntPtr @params)
        {
            _glGetnUniformuiv(program, location, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data);
        internal static glReadnPixels _glReadnPixels;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glReadnPixels", IsExtension=false)]
        public static void ReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data)
        {
            _glReadnPixels(x, y, width, height, format, type, bufSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTextureBarrier();
        internal static glTextureBarrier _glTextureBarrier;

        [Version(Group="GL_VERSION_4_5", Version = "4.5", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTextureBarrier", IsExtension=false)]
        public static void TextureBarrier()
        {
            _glTextureBarrier();
        }
        

        #endregion


        #endregion

        #region Extensions

        #region GL_AMD_performance_monitor
        public const uint GL_COUNTER_TYPE_AMD = (uint)0x8BC0;
        public const uint GL_COUNTER_RANGE_AMD = (uint)0x8BC1;
        public const uint GL_UNSIGNED_INT64_AMD = (uint)0x8BC2;
        public const uint GL_PERCENTAGE_AMD = (uint)0x8BC3;
        public const uint GL_PERFMON_RESULT_AVAILABLE_AMD = (uint)0x8BC4;
        public const uint GL_PERFMON_RESULT_SIZE_AMD = (uint)0x8BC5;
        public const uint GL_PERFMON_RESULT_AMD = (uint)0x8BC6;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfMonitorGroupsAMD(IntPtr numGroups, int groupsSize, uint[] groups);
        internal static glGetPerfMonitorGroupsAMD _glGetPerfMonitorGroupsAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfMonitorGroupsAMD", IsExtension=true)]
        public static void GetPerfMonitorGroupsAMD(IntPtr numGroups, int groupsSize, uint[] groups)
        {
            _glGetPerfMonitorGroupsAMD(numGroups, groupsSize, groups);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfMonitorCountersAMD(uint group, IntPtr numCounters, IntPtr maxActiveCounters, int counterSize, uint[] counters);
        internal static glGetPerfMonitorCountersAMD _glGetPerfMonitorCountersAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfMonitorCountersAMD", IsExtension=true)]
        public static void GetPerfMonitorCountersAMD(uint group, IntPtr numCounters, IntPtr maxActiveCounters, int counterSize, uint[] counters)
        {
            _glGetPerfMonitorCountersAMD(group, numCounters, maxActiveCounters, counterSize, counters);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfMonitorGroupStringAMD(uint group, int bufSize, IntPtr length, byte[] groupString);
        internal static glGetPerfMonitorGroupStringAMD _glGetPerfMonitorGroupStringAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfMonitorGroupStringAMD", IsExtension=true)]
        public static void GetPerfMonitorGroupStringAMD(uint group, int bufSize, IntPtr length, byte[] groupString)
        {
            _glGetPerfMonitorGroupStringAMD(group, bufSize, length, groupString);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfMonitorCounterStringAMD(uint group, uint counter, int bufSize, IntPtr length, byte[] counterString);
        internal static glGetPerfMonitorCounterStringAMD _glGetPerfMonitorCounterStringAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfMonitorCounterStringAMD", IsExtension=true)]
        public static void GetPerfMonitorCounterStringAMD(uint group, uint counter, int bufSize, IntPtr length, byte[] counterString)
        {
            _glGetPerfMonitorCounterStringAMD(group, counter, bufSize, length, counterString);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfMonitorCounterInfoAMD(uint group, uint counter, uint pname, IntPtr data);
        internal static glGetPerfMonitorCounterInfoAMD _glGetPerfMonitorCounterInfoAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfMonitorCounterInfoAMD", IsExtension=true)]
        public static void GetPerfMonitorCounterInfoAMD(uint group, uint counter, uint pname, IntPtr data)
        {
            _glGetPerfMonitorCounterInfoAMD(group, counter, pname, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGenPerfMonitorsAMD(int n, uint[] monitors);
        internal static glGenPerfMonitorsAMD _glGenPerfMonitorsAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenPerfMonitorsAMD", IsExtension=true)]
        public static void GenPerfMonitorsAMD(int n, uint[] monitors)
        {
            _glGenPerfMonitorsAMD(n, monitors);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeletePerfMonitorsAMD(int n, uint[] monitors);
        internal static glDeletePerfMonitorsAMD _glDeletePerfMonitorsAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeletePerfMonitorsAMD", IsExtension=true)]
        public static void DeletePerfMonitorsAMD(int n, uint[] monitors)
        {
            _glDeletePerfMonitorsAMD(n, monitors);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSelectPerfMonitorCountersAMD(uint monitor, bool enable, uint group, int numCounters, uint[] counterList);
        internal static glSelectPerfMonitorCountersAMD _glSelectPerfMonitorCountersAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSelectPerfMonitorCountersAMD", IsExtension=true)]
        public static void SelectPerfMonitorCountersAMD(uint monitor, bool enable, uint group, int numCounters, uint[] counterList)
        {
            _glSelectPerfMonitorCountersAMD(monitor, enable, group, numCounters, counterList);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBeginPerfMonitorAMD(uint monitor);
        internal static glBeginPerfMonitorAMD _glBeginPerfMonitorAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBeginPerfMonitorAMD", IsExtension=true)]
        public static void BeginPerfMonitorAMD(uint monitor)
        {
            _glBeginPerfMonitorAMD(monitor);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEndPerfMonitorAMD(uint monitor);
        internal static glEndPerfMonitorAMD _glEndPerfMonitorAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEndPerfMonitorAMD", IsExtension=true)]
        public static void EndPerfMonitorAMD(uint monitor)
        {
            _glEndPerfMonitorAMD(monitor);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfMonitorCounterDataAMD(uint monitor, uint pname, int dataSize, uint[] data, IntPtr bytesWritten);
        internal static glGetPerfMonitorCounterDataAMD _glGetPerfMonitorCounterDataAMD;

        [Version(Group="GL_AMD_performance_monitor", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfMonitorCounterDataAMD", IsExtension=true)]
        public static void GetPerfMonitorCounterDataAMD(uint monitor, uint pname, int dataSize, uint[] data, IntPtr bytesWritten)
        {
            _glGetPerfMonitorCounterDataAMD(monitor, pname, dataSize, data, bytesWritten);
        }
        

        #endregion

        #region GL_APPLE_rgb_422
        public const uint GL_RGB_422_APPLE = (uint)0x8A1F;
        public const uint GL_UNSIGNED_SHORT_8_8_APPLE = (uint)0x85BA;
        public const uint GL_UNSIGNED_SHORT_8_8_REV_APPLE = (uint)0x85BB;
        public const uint GL_RGB_RAW_422_APPLE = (uint)0x8A51;


        #endregion

        #region GL_ARB_bindless_texture
        public const uint GL_UNSIGNED_INT64_ARB = (uint)0x140F;

        [SuppressUnmanagedCodeSecurity]
        internal delegate ulong glGetTextureHandleARB(uint texture);
        internal static glGetTextureHandleARB _glGetTextureHandleARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureHandleARB", IsExtension=true)]
        public static ulong GetTextureHandleARB(uint texture)
        {
            ulong data = _glGetTextureHandleARB(texture);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate ulong glGetTextureSamplerHandleARB(uint texture, uint sampler);
        internal static glGetTextureSamplerHandleARB _glGetTextureSamplerHandleARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureSamplerHandleARB", IsExtension=true)]
        public static ulong GetTextureSamplerHandleARB(uint texture, uint sampler)
        {
            ulong data = _glGetTextureSamplerHandleARB(texture, sampler);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeTextureHandleResidentARB(ulong handle);
        internal static glMakeTextureHandleResidentARB _glMakeTextureHandleResidentARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeTextureHandleResidentARB", IsExtension=true)]
        public static void MakeTextureHandleResidentARB(ulong handle)
        {
            _glMakeTextureHandleResidentARB(handle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeTextureHandleNonResidentARB(ulong handle);
        internal static glMakeTextureHandleNonResidentARB _glMakeTextureHandleNonResidentARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeTextureHandleNonResidentARB", IsExtension=true)]
        public static void MakeTextureHandleNonResidentARB(ulong handle)
        {
            _glMakeTextureHandleNonResidentARB(handle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate ulong glGetImageHandleARB(uint texture, int level, bool layered, int layer, uint format);
        internal static glGetImageHandleARB _glGetImageHandleARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetImageHandleARB", IsExtension=true)]
        public static ulong GetImageHandleARB(uint texture, int level, bool layered, int layer, uint format)
        {
            ulong data = _glGetImageHandleARB(texture, level, layered, layer, format);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeImageHandleResidentARB(ulong handle, uint access);
        internal static glMakeImageHandleResidentARB _glMakeImageHandleResidentARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeImageHandleResidentARB", IsExtension=true)]
        public static void MakeImageHandleResidentARB(ulong handle, uint access)
        {
            _glMakeImageHandleResidentARB(handle, access);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeImageHandleNonResidentARB(ulong handle);
        internal static glMakeImageHandleNonResidentARB _glMakeImageHandleNonResidentARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeImageHandleNonResidentARB", IsExtension=true)]
        public static void MakeImageHandleNonResidentARB(ulong handle)
        {
            _glMakeImageHandleNonResidentARB(handle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformHandleui64ARB(int location, ulong value);
        internal static glUniformHandleui64ARB _glUniformHandleui64ARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformHandleui64ARB", IsExtension=true)]
        public static void UniformHandleui64ARB(int location, ulong value)
        {
            _glUniformHandleui64ARB(location, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformHandleui64vARB(int location, int count, ulong[] value);
        internal static glUniformHandleui64vARB _glUniformHandleui64vARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformHandleui64vARB", IsExtension=true)]
        public static void UniformHandleui64vARB(int location, int count, ulong[] value)
        {
            _glUniformHandleui64vARB(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformHandleui64ARB(uint program, int location, ulong value);
        internal static glProgramUniformHandleui64ARB _glProgramUniformHandleui64ARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformHandleui64ARB", IsExtension=true)]
        public static void ProgramUniformHandleui64ARB(uint program, int location, ulong value)
        {
            _glProgramUniformHandleui64ARB(program, location, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformHandleui64vARB(uint program, int location, int count, ulong[] values);
        internal static glProgramUniformHandleui64vARB _glProgramUniformHandleui64vARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformHandleui64vARB", IsExtension=true)]
        public static void ProgramUniformHandleui64vARB(uint program, int location, int count, ulong[] values)
        {
            _glProgramUniformHandleui64vARB(program, location, count, values);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsTextureHandleResidentARB(ulong handle);
        internal static glIsTextureHandleResidentARB _glIsTextureHandleResidentARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsTextureHandleResidentARB", IsExtension=true)]
        public static bool IsTextureHandleResidentARB(ulong handle)
        {
            bool data = _glIsTextureHandleResidentARB(handle);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsImageHandleResidentARB(ulong handle);
        internal static glIsImageHandleResidentARB _glIsImageHandleResidentARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsImageHandleResidentARB", IsExtension=true)]
        public static bool IsImageHandleResidentARB(ulong handle)
        {
            bool data = _glIsImageHandleResidentARB(handle);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL1ui64ARB(uint index, ulong x);
        internal static glVertexAttribL1ui64ARB _glVertexAttribL1ui64ARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL1ui64ARB", IsExtension=true)]
        public static void VertexAttribL1ui64ARB(uint index, ulong x)
        {
            _glVertexAttribL1ui64ARB(index, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glVertexAttribL1ui64vARB(uint index, IntPtr v);
        internal static glVertexAttribL1ui64vARB _glVertexAttribL1ui64vARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glVertexAttribL1ui64vARB", IsExtension=true)]
        public static void VertexAttribL1ui64vARB(uint index, IntPtr v)
        {
            _glVertexAttribL1ui64vARB(index, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetVertexAttribLui64vARB(uint index, uint pname, IntPtr @params);
        internal static glGetVertexAttribLui64vARB _glGetVertexAttribLui64vARB;

        [Version(Group="GL_ARB_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetVertexAttribLui64vARB", IsExtension=true)]
        public static void GetVertexAttribLui64vARB(uint index, uint pname, IntPtr @params)
        {
            _glGetVertexAttribLui64vARB(index, pname, @params);
        }
        

        #endregion

        #region GL_ARB_cl_event
        public const uint GL_SYNC_CL_EVENT_ARB = (uint)0x8240;
        public const uint GL_SYNC_CL_EVENT_COMPLETE_ARB = (uint)0x8241;

        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr glCreateSyncFromCLeventARB(IntPtr context, IntPtr @event, uint flags);
        internal static glCreateSyncFromCLeventARB _glCreateSyncFromCLeventARB;

        [Version(Group="GL_ARB_cl_event", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateSyncFromCLeventARB", IsExtension=true)]
        public static IntPtr CreateSyncFromCLeventARB(IntPtr context, IntPtr @event, uint flags)
        {
            IntPtr data = _glCreateSyncFromCLeventARB(context, @event, flags);
            return data;
        }
        

        #endregion

        #region GL_ARB_compute_variable_group_size
        public const uint GL_MAX_COMPUTE_VARIABLE_GROUP_INVOCATIONS_ARB = (uint)0x9344;
        public const uint GL_MAX_COMPUTE_FIXED_GROUP_INVOCATIONS_ARB = (uint)0x90EB;
        public const uint GL_MAX_COMPUTE_VARIABLE_GROUP_SIZE_ARB = (uint)0x9345;
        public const uint GL_MAX_COMPUTE_FIXED_GROUP_SIZE_ARB = (uint)0x91BF;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDispatchComputeGroupSizeARB(uint num_groups_x, uint num_groups_y, uint num_groups_z, uint group_size_x, uint group_size_y, uint group_size_z);
        internal static glDispatchComputeGroupSizeARB _glDispatchComputeGroupSizeARB;

        [Version(Group="GL_ARB_compute_variable_group_size", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDispatchComputeGroupSizeARB", IsExtension=true)]
        public static void DispatchComputeGroupSizeARB(uint num_groups_x, uint num_groups_y, uint num_groups_z, uint group_size_x, uint group_size_y, uint group_size_z)
        {
            _glDispatchComputeGroupSizeARB(num_groups_x, num_groups_y, num_groups_z, group_size_x, group_size_y, group_size_z);
        }
        

        #endregion

        #region GL_ARB_debug_output
        public const uint GL_DEBUG_OUTPUT_SYNCHRONOUS_ARB = (uint)0x8242;
        public const uint GL_DEBUG_NEXT_LOGGED_MESSAGE_LENGTH_ARB = (uint)0x8243;
        public const uint GL_DEBUG_CALLBACK_FUNCTION_ARB = (uint)0x8244;
        public const uint GL_DEBUG_CALLBACK_USER_PARAM_ARB = (uint)0x8245;
        public const uint GL_DEBUG_SOURCE_API_ARB = (uint)0x8246;
        public const uint GL_DEBUG_SOURCE_WINDOW_SYSTEM_ARB = (uint)0x8247;
        public const uint GL_DEBUG_SOURCE_SHADER_COMPILER_ARB = (uint)0x8248;
        public const uint GL_DEBUG_SOURCE_THIRD_PARTY_ARB = (uint)0x8249;
        public const uint GL_DEBUG_SOURCE_APPLICATION_ARB = (uint)0x824A;
        public const uint GL_DEBUG_SOURCE_OTHER_ARB = (uint)0x824B;
        public const uint GL_DEBUG_TYPE_ERROR_ARB = (uint)0x824C;
        public const uint GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR_ARB = (uint)0x824D;
        public const uint GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR_ARB = (uint)0x824E;
        public const uint GL_DEBUG_TYPE_PORTABILITY_ARB = (uint)0x824F;
        public const uint GL_DEBUG_TYPE_PERFORMANCE_ARB = (uint)0x8250;
        public const uint GL_DEBUG_TYPE_OTHER_ARB = (uint)0x8251;
        public const uint GL_MAX_DEBUG_MESSAGE_LENGTH_ARB = (uint)0x9143;
        public const uint GL_MAX_DEBUG_LOGGED_MESSAGES_ARB = (uint)0x9144;
        public const uint GL_DEBUG_LOGGED_MESSAGES_ARB = (uint)0x9145;
        public const uint GL_DEBUG_SEVERITY_HIGH_ARB = (uint)0x9146;
        public const uint GL_DEBUG_SEVERITY_MEDIUM_ARB = (uint)0x9147;
        public const uint GL_DEBUG_SEVERITY_LOW_ARB = (uint)0x9148;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDebugMessageControlARB(uint source, uint type, uint severity, int count, uint[] ids, bool enabled);
        internal static glDebugMessageControlARB _glDebugMessageControlARB;

        [Version(Group="GL_ARB_debug_output", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDebugMessageControlARB", IsExtension=true)]
        public static void DebugMessageControlARB(uint source, uint type, uint severity, int count, uint[] ids, bool enabled)
        {
            _glDebugMessageControlARB(source, type, severity, count, ids, enabled);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDebugMessageInsertARB(uint source, uint type, uint id, uint severity, int length, string buf);
        internal static glDebugMessageInsertARB _glDebugMessageInsertARB;

        [Version(Group="GL_ARB_debug_output", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDebugMessageInsertARB", IsExtension=true)]
        public static void DebugMessageInsertARB(uint source, uint type, uint id, uint severity, int length, string buf)
        {
            _glDebugMessageInsertARB(source, type, id, severity, length, buf);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDebugMessageCallbackARB(DebugProcArb callback, IntPtr userParam);
        internal static glDebugMessageCallbackARB _glDebugMessageCallbackARB;

        [Version(Group="GL_ARB_debug_output", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDebugMessageCallbackARB", IsExtension=true)]
        public static void DebugMessageCallbackARB(DebugProcArb callback, IntPtr userParam)
        {
            _glDebugMessageCallbackARB(callback, userParam);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetDebugMessageLogARB(uint count, int bufSize, uint[] sources, uint[] types, uint[] ids, uint[] severities, int[] lengths, byte[] messageLog);
        internal static glGetDebugMessageLogARB _glGetDebugMessageLogARB;

        [Version(Group="GL_ARB_debug_output", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetDebugMessageLogARB", IsExtension=true)]
        public static uint GetDebugMessageLogARB(uint count, int bufSize, uint[] sources, uint[] types, uint[] ids, uint[] severities, int[] lengths, byte[] messageLog)
        {
            uint data = _glGetDebugMessageLogARB(count, bufSize, sources, types, ids, severities, lengths, messageLog);
            return data;
        }
        

        #endregion

        #region GL_ARB_draw_buffers_blend

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendEquationiARB(uint buf, uint mode);
        internal static glBlendEquationiARB _glBlendEquationiARB;

        [Version(Group="GL_ARB_draw_buffers_blend", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendEquationiARB", IsExtension=true)]
        public static void BlendEquationiARB(uint buf, uint mode)
        {
            _glBlendEquationiARB(buf, mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendEquationSeparateiARB(uint buf, uint modeRGB, uint modeAlpha);
        internal static glBlendEquationSeparateiARB _glBlendEquationSeparateiARB;

        [Version(Group="GL_ARB_draw_buffers_blend", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendEquationSeparateiARB", IsExtension=true)]
        public static void BlendEquationSeparateiARB(uint buf, uint modeRGB, uint modeAlpha)
        {
            _glBlendEquationSeparateiARB(buf, modeRGB, modeAlpha);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendFunciARB(uint buf, uint src, uint dst);
        internal static glBlendFunciARB _glBlendFunciARB;

        [Version(Group="GL_ARB_draw_buffers_blend", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendFunciARB", IsExtension=true)]
        public static void BlendFunciARB(uint buf, uint src, uint dst)
        {
            _glBlendFunciARB(buf, src, dst);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendFuncSeparateiARB(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);
        internal static glBlendFuncSeparateiARB _glBlendFuncSeparateiARB;

        [Version(Group="GL_ARB_draw_buffers_blend", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendFuncSeparateiARB", IsExtension=true)]
        public static void BlendFuncSeparateiARB(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
        {
            _glBlendFuncSeparateiARB(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
        }
        

        #endregion

        #region GL_ARB_imaging
        public const uint GL_BLEND_COLOR = (uint)0x8005;
        public const uint GL_BLEND_EQUATION = (uint)0x8009;


        #endregion

        #region GL_ARB_indirect_parameters
        public const uint GL_PARAMETER_BUFFER_ARB = (uint)0x80EE;
        public const uint GL_PARAMETER_BUFFER_BINDING_ARB = (uint)0x80EF;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMultiDrawArraysIndirectCountARB(uint mode, IntPtr indirect, IntPtr drawcount, int maxdrawcount, int stride);
        internal static glMultiDrawArraysIndirectCountARB _glMultiDrawArraysIndirectCountARB;

        [Version(Group="GL_ARB_indirect_parameters", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMultiDrawArraysIndirectCountARB", IsExtension=true)]
        public static void MultiDrawArraysIndirectCountARB(uint mode, IntPtr indirect, IntPtr drawcount, int maxdrawcount, int stride)
        {
            _glMultiDrawArraysIndirectCountARB(mode, indirect, drawcount, maxdrawcount, stride);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMultiDrawElementsIndirectCountARB(uint mode, uint type, IntPtr indirect, IntPtr drawcount, int maxdrawcount, int stride);
        internal static glMultiDrawElementsIndirectCountARB _glMultiDrawElementsIndirectCountARB;

        [Version(Group="GL_ARB_indirect_parameters", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMultiDrawElementsIndirectCountARB", IsExtension=true)]
        public static void MultiDrawElementsIndirectCountARB(uint mode, uint type, IntPtr indirect, IntPtr drawcount, int maxdrawcount, int stride)
        {
            _glMultiDrawElementsIndirectCountARB(mode, type, indirect, drawcount, maxdrawcount, stride);
        }
        

        #endregion

        #region GL_ARB_internalformat_query2
        public const uint GL_SRGB_DECODE_ARB = (uint)0x8299;


        #endregion

        #region GL_ARB_pipeline_statistics_query
        public const uint GL_VERTICES_SUBMITTED_ARB = (uint)0x82EE;
        public const uint GL_PRIMITIVES_SUBMITTED_ARB = (uint)0x82EF;
        public const uint GL_VERTEX_SHADER_INVOCATIONS_ARB = (uint)0x82F0;
        public const uint GL_TESS_CONTROL_SHADER_PATCHES_ARB = (uint)0x82F1;
        public const uint GL_TESS_EVALUATION_SHADER_INVOCATIONS_ARB = (uint)0x82F2;
        public const uint GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED_ARB = (uint)0x82F3;
        public const uint GL_FRAGMENT_SHADER_INVOCATIONS_ARB = (uint)0x82F4;
        public const uint GL_COMPUTE_SHADER_INVOCATIONS_ARB = (uint)0x82F5;
        public const uint GL_CLIPPING_INPUT_PRIMITIVES_ARB = (uint)0x82F6;
        public const uint GL_CLIPPING_OUTPUT_PRIMITIVES_ARB = (uint)0x82F7;


        #endregion

        #region GL_ARB_robustness
        public const uint GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT_ARB = (uint)0x00000004;
        public const uint GL_LOSE_CONTEXT_ON_RESET_ARB = (uint)0x8252;
        public const uint GL_GUILTY_CONTEXT_RESET_ARB = (uint)0x8253;
        public const uint GL_INNOCENT_CONTEXT_RESET_ARB = (uint)0x8254;
        public const uint GL_UNKNOWN_CONTEXT_RESET_ARB = (uint)0x8255;
        public const uint GL_RESET_NOTIFICATION_STRATEGY_ARB = (uint)0x8256;
        public const uint GL_NO_RESET_NOTIFICATION_ARB = (uint)0x8261;

        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGetGraphicsResetStatusARB();
        internal static glGetGraphicsResetStatusARB _glGetGraphicsResetStatusARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetGraphicsResetStatusARB", IsExtension=true)]
        public static uint GetGraphicsResetStatusARB()
        {
            uint data = _glGetGraphicsResetStatusARB();
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnTexImageARB(uint target, int level, uint format, uint type, int bufSize, IntPtr img);
        internal static glGetnTexImageARB _glGetnTexImageARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnTexImageARB", IsExtension=true)]
        public static void GetnTexImageARB(uint target, int level, uint format, uint type, int bufSize, IntPtr img)
        {
            _glGetnTexImageARB(target, level, format, type, bufSize, img);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glReadnPixelsARB(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data);
        internal static glReadnPixelsARB _glReadnPixelsARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glReadnPixelsARB", IsExtension=true)]
        public static void ReadnPixelsARB(int x, int y, int width, int height, uint format, uint type, int bufSize, IntPtr data)
        {
            _glReadnPixelsARB(x, y, width, height, format, type, bufSize, data);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnCompressedTexImageARB(uint target, int lod, int bufSize, IntPtr img);
        internal static glGetnCompressedTexImageARB _glGetnCompressedTexImageARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnCompressedTexImageARB", IsExtension=true)]
        public static void GetnCompressedTexImageARB(uint target, int lod, int bufSize, IntPtr img)
        {
            _glGetnCompressedTexImageARB(target, lod, bufSize, img);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformfvARB(uint program, int location, int bufSize, float[] @params);
        internal static glGetnUniformfvARB _glGetnUniformfvARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformfvARB", IsExtension=true)]
        public static void GetnUniformfvARB(uint program, int location, int bufSize, float[] @params)
        {
            _glGetnUniformfvARB(program, location, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformivARB(uint program, int location, int bufSize, int[] @params);
        internal static glGetnUniformivARB _glGetnUniformivARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformivARB", IsExtension=true)]
        public static void GetnUniformivARB(uint program, int location, int bufSize, int[] @params)
        {
            _glGetnUniformivARB(program, location, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformuivARB(uint program, int location, int bufSize, uint[] @params);
        internal static glGetnUniformuivARB _glGetnUniformuivARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformuivARB", IsExtension=true)]
        public static void GetnUniformuivARB(uint program, int location, int bufSize, uint[] @params)
        {
            _glGetnUniformuivARB(program, location, bufSize, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetnUniformdvARB(uint program, int location, int bufSize, double[] @params);
        internal static glGetnUniformdvARB _glGetnUniformdvARB;

        [Version(Group="GL_ARB_robustness", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetnUniformdvARB", IsExtension=true)]
        public static void GetnUniformdvARB(uint program, int location, int bufSize, double[] @params)
        {
            _glGetnUniformdvARB(program, location, bufSize, @params);
        }
        

        #endregion

        #region GL_ARB_sample_shading
        public const uint GL_SAMPLE_SHADING_ARB = (uint)0x8C36;
        public const uint GL_MIN_SAMPLE_SHADING_VALUE_ARB = (uint)0x8C37;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMinSampleShadingARB(float value);
        internal static glMinSampleShadingARB _glMinSampleShadingARB;

        [Version(Group="GL_ARB_sample_shading", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMinSampleShadingARB", IsExtension=true)]
        public static void MinSampleShadingARB(float value)
        {
            _glMinSampleShadingARB(value);
        }
        

        #endregion

        #region GL_ARB_shading_language_include
        public const uint GL_SHADER_INCLUDE_ARB = (uint)0x8DAE;
        public const uint GL_NAMED_STRING_LENGTH_ARB = (uint)0x8DE9;
        public const uint GL_NAMED_STRING_TYPE_ARB = (uint)0x8DEA;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedStringARB(uint type, int namelen, string name, int stringlen, string @string);
        internal static glNamedStringARB _glNamedStringARB;

        [Version(Group="GL_ARB_shading_language_include", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedStringARB", IsExtension=true)]
        public static void NamedStringARB(uint type, int namelen, string name, int stringlen, string @string)
        {
            _glNamedStringARB(type, namelen, name, stringlen, @string);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeleteNamedStringARB(int namelen, string name);
        internal static glDeleteNamedStringARB _glDeleteNamedStringARB;

        [Version(Group="GL_ARB_shading_language_include", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeleteNamedStringARB", IsExtension=true)]
        public static void DeleteNamedStringARB(int namelen, string name)
        {
            _glDeleteNamedStringARB(namelen, name);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCompileShaderIncludeARB(uint shader, int count, string[] path, int[] length);
        internal static glCompileShaderIncludeARB _glCompileShaderIncludeARB;

        [Version(Group="GL_ARB_shading_language_include", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCompileShaderIncludeARB", IsExtension=true)]
        public static void CompileShaderIncludeARB(uint shader, int count, string[] path, int[] length)
        {
            _glCompileShaderIncludeARB(shader, count, path, length);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsNamedStringARB(int namelen, string name);
        internal static glIsNamedStringARB _glIsNamedStringARB;

        [Version(Group="GL_ARB_shading_language_include", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsNamedStringARB", IsExtension=true)]
        public static bool IsNamedStringARB(int namelen, string name)
        {
            bool data = _glIsNamedStringARB(namelen, name);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedStringARB(int namelen, string name, int bufSize, IntPtr stringlen, byte[] @string);
        internal static glGetNamedStringARB _glGetNamedStringARB;

        [Version(Group="GL_ARB_shading_language_include", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedStringARB", IsExtension=true)]
        public static void GetNamedStringARB(int namelen, string name, int bufSize, IntPtr stringlen, byte[] @string)
        {
            _glGetNamedStringARB(namelen, name, bufSize, stringlen, @string);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNamedStringivARB(int namelen, string name, uint pname, int[] @params);
        internal static glGetNamedStringivARB _glGetNamedStringivARB;

        [Version(Group="GL_ARB_shading_language_include", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNamedStringivARB", IsExtension=true)]
        public static void GetNamedStringivARB(int namelen, string name, uint pname, int[] @params)
        {
            _glGetNamedStringivARB(namelen, name, pname, @params);
        }
        

        #endregion

        #region GL_ARB_sparse_buffer
        public const uint GL_SPARSE_STORAGE_BIT_ARB = (uint)0x0400;
        public const uint GL_SPARSE_BUFFER_PAGE_SIZE_ARB = (uint)0x82F8;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBufferPageCommitmentARB(uint target, IntPtr offset, IntPtr size, bool commit);
        internal static glBufferPageCommitmentARB _glBufferPageCommitmentARB;

        [Version(Group="GL_ARB_sparse_buffer", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBufferPageCommitmentARB", IsExtension=true)]
        public static void BufferPageCommitmentARB(uint target, IntPtr offset, IntPtr size, bool commit)
        {
            _glBufferPageCommitmentARB(target, offset, size, commit);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedBufferPageCommitmentEXT(uint buffer, IntPtr offset, IntPtr size, bool commit);
        internal static glNamedBufferPageCommitmentEXT _glNamedBufferPageCommitmentEXT;

        [Version(Group="GL_ARB_sparse_buffer", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedBufferPageCommitmentEXT", IsExtension=true)]
        public static void NamedBufferPageCommitmentEXT(uint buffer, IntPtr offset, IntPtr size, bool commit)
        {
            _glNamedBufferPageCommitmentEXT(buffer, offset, size, commit);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedBufferPageCommitmentARB(uint buffer, IntPtr offset, IntPtr size, bool commit);
        internal static glNamedBufferPageCommitmentARB _glNamedBufferPageCommitmentARB;

        [Version(Group="GL_ARB_sparse_buffer", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedBufferPageCommitmentARB", IsExtension=true)]
        public static void NamedBufferPageCommitmentARB(uint buffer, IntPtr offset, IntPtr size, bool commit)
        {
            _glNamedBufferPageCommitmentARB(buffer, offset, size, commit);
        }
        

        #endregion

        #region GL_ARB_sparse_texture
        public const uint GL_TEXTURE_SPARSE_ARB = (uint)0x91A6;
        public const uint GL_VIRTUAL_PAGE_SIZE_INDEX_ARB = (uint)0x91A7;
        public const uint GL_NUM_SPARSE_LEVELS_ARB = (uint)0x91AA;
        public const uint GL_NUM_VIRTUAL_PAGE_SIZES_ARB = (uint)0x91A8;
        public const uint GL_VIRTUAL_PAGE_SIZE_X_ARB = (uint)0x9195;
        public const uint GL_VIRTUAL_PAGE_SIZE_Y_ARB = (uint)0x9196;
        public const uint GL_VIRTUAL_PAGE_SIZE_Z_ARB = (uint)0x9197;
        public const uint GL_MAX_SPARSE_TEXTURE_SIZE_ARB = (uint)0x9198;
        public const uint GL_MAX_SPARSE_3D_TEXTURE_SIZE_ARB = (uint)0x9199;
        public const uint GL_MAX_SPARSE_ARRAY_TEXTURE_LAYERS_ARB = (uint)0x919A;
        public const uint GL_SPARSE_TEXTURE_FULL_ARRAY_CUBE_MIPMAPS_ARB = (uint)0x91A9;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTexPageCommitmentARB(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, bool commit);
        internal static glTexPageCommitmentARB _glTexPageCommitmentARB;

        [Version(Group="GL_ARB_sparse_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTexPageCommitmentARB", IsExtension=true)]
        public static void TexPageCommitmentARB(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, bool commit)
        {
            _glTexPageCommitmentARB(target, level, xoffset, yoffset, zoffset, width, height, depth, commit);
        }
        

        #endregion

        #region GL_ARB_texture_compression_bptc
        public const uint GL_COMPRESSED_RGBA_BPTC_UNORM_ARB = (uint)0x8E8C;
        public const uint GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM_ARB = (uint)0x8E8D;
        public const uint GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT_ARB = (uint)0x8E8E;
        public const uint GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT_ARB = (uint)0x8E8F;


        #endregion

        #region GL_ARB_texture_cube_map_array
        public const uint GL_TEXTURE_CUBE_MAP_ARRAY_ARB = (uint)0x9009;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP_ARRAY_ARB = (uint)0x900A;
        public const uint GL_PROXY_TEXTURE_CUBE_MAP_ARRAY_ARB = (uint)0x900B;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY_ARB = (uint)0x900C;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW_ARB = (uint)0x900D;
        public const uint GL_INT_SAMPLER_CUBE_MAP_ARRAY_ARB = (uint)0x900E;
        public const uint GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY_ARB = (uint)0x900F;


        #endregion

        #region GL_ARB_texture_gather
        public const uint GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET_ARB = (uint)0x8E5E;
        public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET_ARB = (uint)0x8E5F;
        public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_COMPONENTS_ARB = (uint)0x8F9F;


        #endregion

        #region GL_ARB_transform_feedback_overflow_query
        public const uint GL_TRANSFORM_FEEDBACK_OVERFLOW_ARB = (uint)0x82EC;
        public const uint GL_TRANSFORM_FEEDBACK_STREAM_OVERFLOW_ARB = (uint)0x82ED;


        #endregion

        #region GL_EXT_debug_label
        public const uint GL_PROGRAM_PIPELINE_OBJECT_EXT = (uint)0x8A4F;
        public const uint GL_PROGRAM_OBJECT_EXT = (uint)0x8B40;
        public const uint GL_SHADER_OBJECT_EXT = (uint)0x8B48;
        public const uint GL_BUFFER_OBJECT_EXT = (uint)0x9151;
        public const uint GL_QUERY_OBJECT_EXT = (uint)0x9153;
        public const uint GL_VERTEX_ARRAY_OBJECT_EXT = (uint)0x9154;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glLabelObjectEXT(uint type, uint @object, int length, IntPtr label);
        internal static glLabelObjectEXT _glLabelObjectEXT;

        [Version(Group="GL_EXT_debug_label", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glLabelObjectEXT", IsExtension=true)]
        public static void LabelObjectEXT(uint type, uint @object, int length, IntPtr label)
        {
            _glLabelObjectEXT(type, @object, length, label);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetObjectLabelEXT(uint type, uint @object, int bufSize, IntPtr length, byte[] label);
        internal static glGetObjectLabelEXT _glGetObjectLabelEXT;

        [Version(Group="GL_EXT_debug_label", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetObjectLabelEXT", IsExtension=true)]
        public static void GetObjectLabelEXT(uint type, uint @object, int bufSize, IntPtr length, byte[] label)
        {
            _glGetObjectLabelEXT(type, @object, bufSize, length, label);
        }
        

        #endregion

        #region GL_EXT_debug_marker

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInsertEventMarkerEXT(int length, IntPtr marker);
        internal static glInsertEventMarkerEXT _glInsertEventMarkerEXT;

        [Version(Group="GL_EXT_debug_marker", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInsertEventMarkerEXT", IsExtension=true)]
        public static void InsertEventMarkerEXT(int length, IntPtr marker)
        {
            _glInsertEventMarkerEXT(length, marker);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPushGroupMarkerEXT(int length, IntPtr marker);
        internal static glPushGroupMarkerEXT _glPushGroupMarkerEXT;

        [Version(Group="GL_EXT_debug_marker", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPushGroupMarkerEXT", IsExtension=true)]
        public static void PushGroupMarkerEXT(int length, IntPtr marker)
        {
            _glPushGroupMarkerEXT(length, marker);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPopGroupMarkerEXT();
        internal static glPopGroupMarkerEXT _glPopGroupMarkerEXT;

        [Version(Group="GL_EXT_debug_marker", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPopGroupMarkerEXT", IsExtension=true)]
        public static void PopGroupMarkerEXT()
        {
            _glPopGroupMarkerEXT();
        }
        

        #endregion

        #region GL_EXT_draw_instanced

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawArraysInstancedEXT(uint mode, int start, int count, int primcount);
        internal static glDrawArraysInstancedEXT _glDrawArraysInstancedEXT;

        [Version(Group="GL_EXT_draw_instanced", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawArraysInstancedEXT", IsExtension=true)]
        public static void DrawArraysInstancedEXT(uint mode, int start, int count, int primcount)
        {
            _glDrawArraysInstancedEXT(mode, start, count, primcount);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDrawElementsInstancedEXT(uint mode, int count, uint type, IntPtr indices, int primcount);
        internal static glDrawElementsInstancedEXT _glDrawElementsInstancedEXT;

        [Version(Group="GL_EXT_draw_instanced", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDrawElementsInstancedEXT", IsExtension=true)]
        public static void DrawElementsInstancedEXT(uint mode, int count, uint type, IntPtr indices, int primcount)
        {
            _glDrawElementsInstancedEXT(mode, count, type, indices, primcount);
        }
        

        #endregion

        #region GL_EXT_polygon_offset_clamp
        public const uint GL_POLYGON_OFFSET_CLAMP_EXT = (uint)0x8E1B;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPolygonOffsetClampEXT(float factor, float units, float clamp);
        internal static glPolygonOffsetClampEXT _glPolygonOffsetClampEXT;

        [Version(Group="GL_EXT_polygon_offset_clamp", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPolygonOffsetClampEXT", IsExtension=true)]
        public static void PolygonOffsetClampEXT(float factor, float units, float clamp)
        {
            _glPolygonOffsetClampEXT(factor, units, clamp);
        }
        

        #endregion

        #region GL_EXT_raster_multisample
        public const uint GL_RASTER_MULTISAMPLE_EXT = (uint)0x9327;
        public const uint GL_RASTER_SAMPLES_EXT = (uint)0x9328;
        public const uint GL_MAX_RASTER_SAMPLES_EXT = (uint)0x9329;
        public const uint GL_RASTER_FIXED_SAMPLE_LOCATIONS_EXT = (uint)0x932A;
        public const uint GL_MULTISAMPLE_RASTERIZATION_ALLOWED_EXT = (uint)0x932B;
        public const uint GL_EFFECTIVE_RASTER_SAMPLES_EXT = (uint)0x932C;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glRasterSamplesEXT(uint samples, bool fixedsamplelocations);
        internal static glRasterSamplesEXT _glRasterSamplesEXT;

        [Version(Group="GL_EXT_raster_multisample", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glRasterSamplesEXT", IsExtension=true)]
        public static void RasterSamplesEXT(uint samples, bool fixedsamplelocations)
        {
            _glRasterSamplesEXT(samples, fixedsamplelocations);
        }
        

        #endregion

        #region GL_EXT_separate_shader_objects
        public const uint GL_ACTIVE_PROGRAM_EXT = (uint)0x8B8D;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUseShaderProgramEXT(uint type, uint program);
        internal static glUseShaderProgramEXT _glUseShaderProgramEXT;

        [Version(Group="GL_EXT_separate_shader_objects", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUseShaderProgramEXT", IsExtension=true)]
        public static void UseShaderProgramEXT(uint type, uint program)
        {
            _glUseShaderProgramEXT(type, program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glActiveProgramEXT(uint program);
        internal static glActiveProgramEXT _glActiveProgramEXT;

        [Version(Group="GL_EXT_separate_shader_objects", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glActiveProgramEXT", IsExtension=true)]
        public static void ActiveProgramEXT(uint program)
        {
            _glActiveProgramEXT(program);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glCreateShaderProgramEXT(uint type, IntPtr @string);
        internal static glCreateShaderProgramEXT _glCreateShaderProgramEXT;

        [Version(Group="GL_EXT_separate_shader_objects", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreateShaderProgramEXT", IsExtension=true)]
        public static uint CreateShaderProgramEXT(uint type, IntPtr @string)
        {
            uint data = _glCreateShaderProgramEXT(type, @string);
            return data;
        }
        

        #endregion

        #region GL_EXT_texture_compression_s3tc
        public const uint GL_COMPRESSED_RGB_S3TC_DXT1_EXT = (uint)0x83F0;
        public const uint GL_COMPRESSED_RGBA_S3TC_DXT1_EXT = (uint)0x83F1;
        public const uint GL_COMPRESSED_RGBA_S3TC_DXT3_EXT = (uint)0x83F2;
        public const uint GL_COMPRESSED_RGBA_S3TC_DXT5_EXT = (uint)0x83F3;


        #endregion

        #region GL_EXT_texture_sRGB_decode
        public const uint GL_TEXTURE_SRGB_DECODE_EXT = (uint)0x8A48;
        public const uint GL_DECODE_EXT = (uint)0x8A49;
        public const uint GL_SKIP_DECODE_EXT = (uint)0x8A4A;


        #endregion

        #region GL_EXT_window_rectangles
        public const uint GL_INCLUSIVE_EXT = (uint)0x8F10;
        public const uint GL_EXCLUSIVE_EXT = (uint)0x8F11;
        public const uint GL_WINDOW_RECTANGLE_EXT = (uint)0x8F12;
        public const uint GL_WINDOW_RECTANGLE_MODE_EXT = (uint)0x8F13;
        public const uint GL_MAX_WINDOW_RECTANGLES_EXT = (uint)0x8F14;
        public const uint GL_NUM_WINDOW_RECTANGLES_EXT = (uint)0x8F15;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glWindowRectanglesEXT(uint mode, int count, int[] box);
        internal static glWindowRectanglesEXT _glWindowRectanglesEXT;

        [Version(Group="GL_EXT_window_rectangles", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glWindowRectanglesEXT", IsExtension=true)]
        public static void WindowRectanglesEXT(uint mode, int count, int[] box)
        {
            _glWindowRectanglesEXT(mode, count, box);
        }
        

        #endregion

        #region GL_INTEL_conservative_rasterization
        public const uint GL_CONSERVATIVE_RASTERIZATION_INTEL = (uint)0x83FE;


        #endregion

        #region GL_INTEL_framebuffer_CMAA

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glApplyFramebufferAttachmentCMAAINTEL();
        internal static glApplyFramebufferAttachmentCMAAINTEL _glApplyFramebufferAttachmentCMAAINTEL;

        [Version(Group="GL_INTEL_framebuffer_CMAA", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glApplyFramebufferAttachmentCMAAINTEL", IsExtension=true)]
        public static void ApplyFramebufferAttachmentCMAAINTEL()
        {
            _glApplyFramebufferAttachmentCMAAINTEL();
        }
        

        #endregion

        #region GL_INTEL_performance_query
        public const uint GL_PERFQUERY_SINGLE_CONTEXT_INTEL = (uint)0x00000000;
        public const uint GL_PERFQUERY_GLOBAL_CONTEXT_INTEL = (uint)0x00000001;
        public const uint GL_PERFQUERY_WAIT_INTEL = (uint)0x83FB;
        public const uint GL_PERFQUERY_FLUSH_INTEL = (uint)0x83FA;
        public const uint GL_PERFQUERY_DONOT_FLUSH_INTEL = (uint)0x83F9;
        public const uint GL_PERFQUERY_COUNTER_EVENT_INTEL = (uint)0x94F0;
        public const uint GL_PERFQUERY_COUNTER_DURATION_NORM_INTEL = (uint)0x94F1;
        public const uint GL_PERFQUERY_COUNTER_DURATION_RAW_INTEL = (uint)0x94F2;
        public const uint GL_PERFQUERY_COUNTER_THROUGHPUT_INTEL = (uint)0x94F3;
        public const uint GL_PERFQUERY_COUNTER_RAW_INTEL = (uint)0x94F4;
        public const uint GL_PERFQUERY_COUNTER_TIMESTAMP_INTEL = (uint)0x94F5;
        public const uint GL_PERFQUERY_COUNTER_DATA_UINT32_INTEL = (uint)0x94F8;
        public const uint GL_PERFQUERY_COUNTER_DATA_UINT64_INTEL = (uint)0x94F9;
        public const uint GL_PERFQUERY_COUNTER_DATA_FLOAT_INTEL = (uint)0x94FA;
        public const uint GL_PERFQUERY_COUNTER_DATA_DOUBLE_INTEL = (uint)0x94FB;
        public const uint GL_PERFQUERY_COUNTER_DATA_BOOL32_INTEL = (uint)0x94FC;
        public const uint GL_PERFQUERY_QUERY_NAME_LENGTH_MAX_INTEL = (uint)0x94FD;
        public const uint GL_PERFQUERY_COUNTER_NAME_LENGTH_MAX_INTEL = (uint)0x94FE;
        public const uint GL_PERFQUERY_COUNTER_DESC_LENGTH_MAX_INTEL = (uint)0x94FF;
        public const uint GL_PERFQUERY_GPA_EXTENDED_COUNTERS_INTEL = (uint)0x9500;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBeginPerfQueryINTEL(uint queryHandle);
        internal static glBeginPerfQueryINTEL _glBeginPerfQueryINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBeginPerfQueryINTEL", IsExtension=true)]
        public static void BeginPerfQueryINTEL(uint queryHandle)
        {
            _glBeginPerfQueryINTEL(queryHandle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCreatePerfQueryINTEL(uint queryId, IntPtr queryHandle);
        internal static glCreatePerfQueryINTEL _glCreatePerfQueryINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCreatePerfQueryINTEL", IsExtension=true)]
        public static void CreatePerfQueryINTEL(uint queryId, IntPtr queryHandle)
        {
            _glCreatePerfQueryINTEL(queryId, queryHandle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeletePerfQueryINTEL(uint queryHandle);
        internal static glDeletePerfQueryINTEL _glDeletePerfQueryINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeletePerfQueryINTEL", IsExtension=true)]
        public static void DeletePerfQueryINTEL(uint queryHandle)
        {
            _glDeletePerfQueryINTEL(queryHandle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEndPerfQueryINTEL(uint queryHandle);
        internal static glEndPerfQueryINTEL _glEndPerfQueryINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEndPerfQueryINTEL", IsExtension=true)]
        public static void EndPerfQueryINTEL(uint queryHandle)
        {
            _glEndPerfQueryINTEL(queryHandle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetFirstPerfQueryIdINTEL(IntPtr queryId);
        internal static glGetFirstPerfQueryIdINTEL _glGetFirstPerfQueryIdINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetFirstPerfQueryIdINTEL", IsExtension=true)]
        public static void GetFirstPerfQueryIdINTEL(IntPtr queryId)
        {
            _glGetFirstPerfQueryIdINTEL(queryId);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetNextPerfQueryIdINTEL(uint queryId, IntPtr nextQueryId);
        internal static glGetNextPerfQueryIdINTEL _glGetNextPerfQueryIdINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetNextPerfQueryIdINTEL", IsExtension=true)]
        public static void GetNextPerfQueryIdINTEL(uint queryId, IntPtr nextQueryId)
        {
            _glGetNextPerfQueryIdINTEL(queryId, nextQueryId);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfCounterInfoINTEL(uint queryId, uint counterId, uint counterNameLength, IntPtr counterName, uint counterDescLength, IntPtr counterDesc, IntPtr counterOffset, IntPtr counterDataSize, IntPtr counterTypeEnum, IntPtr counterDataTypeEnum, IntPtr rawCounterMaxValue);
        internal static glGetPerfCounterInfoINTEL _glGetPerfCounterInfoINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfCounterInfoINTEL", IsExtension=true)]
        public static void GetPerfCounterInfoINTEL(uint queryId, uint counterId, uint counterNameLength, IntPtr counterName, uint counterDescLength, IntPtr counterDesc, IntPtr counterOffset, IntPtr counterDataSize, IntPtr counterTypeEnum, IntPtr counterDataTypeEnum, IntPtr rawCounterMaxValue)
        {
            _glGetPerfCounterInfoINTEL(queryId, counterId, counterNameLength, counterName, counterDescLength, counterDesc, counterOffset, counterDataSize, counterTypeEnum, counterDataTypeEnum, rawCounterMaxValue);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfQueryDataINTEL(uint queryHandle, uint flags, int dataSize, IntPtr data, IntPtr bytesWritten);
        internal static glGetPerfQueryDataINTEL _glGetPerfQueryDataINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfQueryDataINTEL", IsExtension=true)]
        public static void GetPerfQueryDataINTEL(uint queryHandle, uint flags, int dataSize, IntPtr data, IntPtr bytesWritten)
        {
            _glGetPerfQueryDataINTEL(queryHandle, flags, dataSize, data, bytesWritten);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfQueryIdByNameINTEL(IntPtr queryName, IntPtr queryId);
        internal static glGetPerfQueryIdByNameINTEL _glGetPerfQueryIdByNameINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfQueryIdByNameINTEL", IsExtension=true)]
        public static void GetPerfQueryIdByNameINTEL(IntPtr queryName, IntPtr queryId)
        {
            _glGetPerfQueryIdByNameINTEL(queryName, queryId);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPerfQueryInfoINTEL(uint queryId, uint queryNameLength, IntPtr queryName, IntPtr dataSize, IntPtr noCounters, IntPtr noInstances, IntPtr capsMask);
        internal static glGetPerfQueryInfoINTEL _glGetPerfQueryInfoINTEL;

        [Version(Group="GL_INTEL_performance_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPerfQueryInfoINTEL", IsExtension=true)]
        public static void GetPerfQueryInfoINTEL(uint queryId, uint queryNameLength, IntPtr queryName, IntPtr dataSize, IntPtr noCounters, IntPtr noInstances, IntPtr capsMask)
        {
            _glGetPerfQueryInfoINTEL(queryId, queryNameLength, queryName, dataSize, noCounters, noInstances, capsMask);
        }
        

        #endregion

        #region GL_KHR_blend_equation_advanced
        public const uint GL_MULTIPLY_KHR = (uint)0x9294;
        public const uint GL_SCREEN_KHR = (uint)0x9295;
        public const uint GL_OVERLAY_KHR = (uint)0x9296;
        public const uint GL_DARKEN_KHR = (uint)0x9297;
        public const uint GL_LIGHTEN_KHR = (uint)0x9298;
        public const uint GL_COLORDODGE_KHR = (uint)0x9299;
        public const uint GL_COLORBURN_KHR = (uint)0x929A;
        public const uint GL_HARDLIGHT_KHR = (uint)0x929B;
        public const uint GL_SOFTLIGHT_KHR = (uint)0x929C;
        public const uint GL_DIFFERENCE_KHR = (uint)0x929E;
        public const uint GL_EXCLUSION_KHR = (uint)0x92A0;
        public const uint GL_HSL_HUE_KHR = (uint)0x92AD;
        public const uint GL_HSL_SATURATION_KHR = (uint)0x92AE;
        public const uint GL_HSL_COLOR_KHR = (uint)0x92AF;
        public const uint GL_HSL_LUMINOSITY_KHR = (uint)0x92B0;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendBarrierKHR();
        internal static glBlendBarrierKHR _glBlendBarrierKHR;

        [Version(Group="GL_KHR_blend_equation_advanced", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendBarrierKHR", IsExtension=true)]
        public static void BlendBarrierKHR()
        {
            _glBlendBarrierKHR();
        }
        

        #endregion

        #region GL_KHR_blend_equation_advanced_coherent
        public const uint GL_BLEND_ADVANCED_COHERENT_KHR = (uint)0x9285;


        #endregion

        #region GL_KHR_no_error
        public const uint GL_CONTEXT_FLAG_NO_ERROR_BIT_KHR = (uint)0x00000008;


        #endregion

        #region GL_KHR_robustness
        public const uint GL_CONTEXT_ROBUST_ACCESS = (uint)0x90F3;


        #endregion

        #region GL_KHR_texture_compression_astc_hdr
        public const uint GL_COMPRESSED_RGBA_ASTC_4x4_KHR = (uint)0x93B0;
        public const uint GL_COMPRESSED_RGBA_ASTC_5x4_KHR = (uint)0x93B1;
        public const uint GL_COMPRESSED_RGBA_ASTC_5x5_KHR = (uint)0x93B2;
        public const uint GL_COMPRESSED_RGBA_ASTC_6x5_KHR = (uint)0x93B3;
        public const uint GL_COMPRESSED_RGBA_ASTC_6x6_KHR = (uint)0x93B4;
        public const uint GL_COMPRESSED_RGBA_ASTC_8x5_KHR = (uint)0x93B5;
        public const uint GL_COMPRESSED_RGBA_ASTC_8x6_KHR = (uint)0x93B6;
        public const uint GL_COMPRESSED_RGBA_ASTC_8x8_KHR = (uint)0x93B7;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x5_KHR = (uint)0x93B8;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x6_KHR = (uint)0x93B9;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x8_KHR = (uint)0x93BA;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x10_KHR = (uint)0x93BB;
        public const uint GL_COMPRESSED_RGBA_ASTC_12x10_KHR = (uint)0x93BC;
        public const uint GL_COMPRESSED_RGBA_ASTC_12x12_KHR = (uint)0x93BD;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4_KHR = (uint)0x93D0;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4_KHR = (uint)0x93D1;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5_KHR = (uint)0x93D2;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5_KHR = (uint)0x93D3;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6_KHR = (uint)0x93D4;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x5_KHR = (uint)0x93D5;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x6_KHR = (uint)0x93D6;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x8_KHR = (uint)0x93D7;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x5_KHR = (uint)0x93D8;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x6_KHR = (uint)0x93D9;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x8_KHR = (uint)0x93DA;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x10_KHR = (uint)0x93DB;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x10_KHR = (uint)0x93DC;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x12_KHR = (uint)0x93DD;


        #endregion

        #region GL_NV_bindless_texture

        [SuppressUnmanagedCodeSecurity]
        internal delegate ulong glGetTextureHandleNV(uint texture);
        internal static glGetTextureHandleNV _glGetTextureHandleNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureHandleNV", IsExtension=true)]
        public static ulong GetTextureHandleNV(uint texture)
        {
            ulong data = _glGetTextureHandleNV(texture);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate ulong glGetTextureSamplerHandleNV(uint texture, uint sampler);
        internal static glGetTextureSamplerHandleNV _glGetTextureSamplerHandleNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetTextureSamplerHandleNV", IsExtension=true)]
        public static ulong GetTextureSamplerHandleNV(uint texture, uint sampler)
        {
            ulong data = _glGetTextureSamplerHandleNV(texture, sampler);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeTextureHandleResidentNV(ulong handle);
        internal static glMakeTextureHandleResidentNV _glMakeTextureHandleResidentNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeTextureHandleResidentNV", IsExtension=true)]
        public static void MakeTextureHandleResidentNV(ulong handle)
        {
            _glMakeTextureHandleResidentNV(handle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeTextureHandleNonResidentNV(ulong handle);
        internal static glMakeTextureHandleNonResidentNV _glMakeTextureHandleNonResidentNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeTextureHandleNonResidentNV", IsExtension=true)]
        public static void MakeTextureHandleNonResidentNV(ulong handle)
        {
            _glMakeTextureHandleNonResidentNV(handle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate ulong glGetImageHandleNV(uint texture, int level, bool layered, int layer, uint format);
        internal static glGetImageHandleNV _glGetImageHandleNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetImageHandleNV", IsExtension=true)]
        public static ulong GetImageHandleNV(uint texture, int level, bool layered, int layer, uint format)
        {
            ulong data = _glGetImageHandleNV(texture, level, layered, layer, format);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeImageHandleResidentNV(ulong handle, uint access);
        internal static glMakeImageHandleResidentNV _glMakeImageHandleResidentNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeImageHandleResidentNV", IsExtension=true)]
        public static void MakeImageHandleResidentNV(ulong handle, uint access)
        {
            _glMakeImageHandleResidentNV(handle, access);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMakeImageHandleNonResidentNV(ulong handle);
        internal static glMakeImageHandleNonResidentNV _glMakeImageHandleNonResidentNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMakeImageHandleNonResidentNV", IsExtension=true)]
        public static void MakeImageHandleNonResidentNV(ulong handle)
        {
            _glMakeImageHandleNonResidentNV(handle);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformHandleui64NV(int location, ulong value);
        internal static glUniformHandleui64NV _glUniformHandleui64NV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformHandleui64NV", IsExtension=true)]
        public static void UniformHandleui64NV(int location, ulong value)
        {
            _glUniformHandleui64NV(location, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniformHandleui64vNV(int location, int count, ulong[] value);
        internal static glUniformHandleui64vNV _glUniformHandleui64vNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniformHandleui64vNV", IsExtension=true)]
        public static void UniformHandleui64vNV(int location, int count, ulong[] value)
        {
            _glUniformHandleui64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformHandleui64NV(uint program, int location, ulong value);
        internal static glProgramUniformHandleui64NV _glProgramUniformHandleui64NV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformHandleui64NV", IsExtension=true)]
        public static void ProgramUniformHandleui64NV(uint program, int location, ulong value)
        {
            _glProgramUniformHandleui64NV(program, location, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniformHandleui64vNV(uint program, int location, int count, ulong[] values);
        internal static glProgramUniformHandleui64vNV _glProgramUniformHandleui64vNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniformHandleui64vNV", IsExtension=true)]
        public static void ProgramUniformHandleui64vNV(uint program, int location, int count, ulong[] values)
        {
            _glProgramUniformHandleui64vNV(program, location, count, values);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsTextureHandleResidentNV(ulong handle);
        internal static glIsTextureHandleResidentNV _glIsTextureHandleResidentNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsTextureHandleResidentNV", IsExtension=true)]
        public static bool IsTextureHandleResidentNV(ulong handle)
        {
            bool data = _glIsTextureHandleResidentNV(handle);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsImageHandleResidentNV(ulong handle);
        internal static glIsImageHandleResidentNV _glIsImageHandleResidentNV;

        [Version(Group="GL_NV_bindless_texture", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsImageHandleResidentNV", IsExtension=true)]
        public static bool IsImageHandleResidentNV(ulong handle)
        {
            bool data = _glIsImageHandleResidentNV(handle);
            return data;
        }
        

        #endregion

        #region GL_NV_blend_equation_advanced
        public const uint GL_BLEND_OVERLAP_NV = (uint)0x9281;
        public const uint GL_BLEND_PREMULTIPLIED_SRC_NV = (uint)0x9280;
        public const uint GL_BLUE_NV = (uint)0x1905;
        public const uint GL_COLORBURN_NV = (uint)0x929A;
        public const uint GL_COLORDODGE_NV = (uint)0x9299;
        public const uint GL_CONJOINT_NV = (uint)0x9284;
        public const uint GL_CONTRAST_NV = (uint)0x92A1;
        public const uint GL_DARKEN_NV = (uint)0x9297;
        public const uint GL_DIFFERENCE_NV = (uint)0x929E;
        public const uint GL_DISJOINT_NV = (uint)0x9283;
        public const uint GL_DST_ATOP_NV = (uint)0x928F;
        public const uint GL_DST_IN_NV = (uint)0x928B;
        public const uint GL_DST_NV = (uint)0x9287;
        public const uint GL_DST_OUT_NV = (uint)0x928D;
        public const uint GL_DST_OVER_NV = (uint)0x9289;
        public const uint GL_EXCLUSION_NV = (uint)0x92A0;
        public const uint GL_GREEN_NV = (uint)0x1904;
        public const uint GL_HARDLIGHT_NV = (uint)0x929B;
        public const uint GL_HARDMIX_NV = (uint)0x92A9;
        public const uint GL_HSL_COLOR_NV = (uint)0x92AF;
        public const uint GL_HSL_HUE_NV = (uint)0x92AD;
        public const uint GL_HSL_LUMINOSITY_NV = (uint)0x92B0;
        public const uint GL_HSL_SATURATION_NV = (uint)0x92AE;
        public const uint GL_INVERT_OVG_NV = (uint)0x92B4;
        public const uint GL_INVERT_RGB_NV = (uint)0x92A3;
        public const uint GL_LIGHTEN_NV = (uint)0x9298;
        public const uint GL_LINEARBURN_NV = (uint)0x92A5;
        public const uint GL_LINEARDODGE_NV = (uint)0x92A4;
        public const uint GL_LINEARLIGHT_NV = (uint)0x92A7;
        public const uint GL_MINUS_CLAMPED_NV = (uint)0x92B3;
        public const uint GL_MINUS_NV = (uint)0x929F;
        public const uint GL_MULTIPLY_NV = (uint)0x9294;
        public const uint GL_OVERLAY_NV = (uint)0x9296;
        public const uint GL_PINLIGHT_NV = (uint)0x92A8;
        public const uint GL_PLUS_CLAMPED_ALPHA_NV = (uint)0x92B2;
        public const uint GL_PLUS_CLAMPED_NV = (uint)0x92B1;
        public const uint GL_PLUS_DARKER_NV = (uint)0x9292;
        public const uint GL_PLUS_NV = (uint)0x9291;
        public const uint GL_RED_NV = (uint)0x1903;
        public const uint GL_SCREEN_NV = (uint)0x9295;
        public const uint GL_SOFTLIGHT_NV = (uint)0x929C;
        public const uint GL_SRC_ATOP_NV = (uint)0x928E;
        public const uint GL_SRC_IN_NV = (uint)0x928A;
        public const uint GL_SRC_NV = (uint)0x9286;
        public const uint GL_SRC_OUT_NV = (uint)0x928C;
        public const uint GL_SRC_OVER_NV = (uint)0x9288;
        public const uint GL_UNCORRELATED_NV = (uint)0x9282;
        public const uint GL_VIVIDLIGHT_NV = (uint)0x92A6;
        public const uint GL_XOR_NV = (uint)0x1506;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendParameteriNV(uint pname, int value);
        internal static glBlendParameteriNV _glBlendParameteriNV;

        [Version(Group="GL_NV_blend_equation_advanced", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendParameteriNV", IsExtension=true)]
        public static void BlendParameteriNV(uint pname, int value)
        {
            _glBlendParameteriNV(pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBlendBarrierNV();
        internal static glBlendBarrierNV _glBlendBarrierNV;

        [Version(Group="GL_NV_blend_equation_advanced", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBlendBarrierNV", IsExtension=true)]
        public static void BlendBarrierNV()
        {
            _glBlendBarrierNV();
        }
        

        #endregion

        #region GL_NV_blend_equation_advanced_coherent
        public const uint GL_BLEND_ADVANCED_COHERENT_NV = (uint)0x9285;


        #endregion

        #region GL_NV_conditional_render
        public const uint GL_QUERY_WAIT_NV = (uint)0x8E13;
        public const uint GL_QUERY_NO_WAIT_NV = (uint)0x8E14;
        public const uint GL_QUERY_BY_REGION_WAIT_NV = (uint)0x8E15;
        public const uint GL_QUERY_BY_REGION_NO_WAIT_NV = (uint)0x8E16;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glBeginConditionalRenderNV(uint id, uint mode);
        internal static glBeginConditionalRenderNV _glBeginConditionalRenderNV;

        [Version(Group="GL_NV_conditional_render", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glBeginConditionalRenderNV", IsExtension=true)]
        public static void BeginConditionalRenderNV(uint id, uint mode)
        {
            _glBeginConditionalRenderNV(id, mode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glEndConditionalRenderNV();
        internal static glEndConditionalRenderNV _glEndConditionalRenderNV;

        [Version(Group="GL_NV_conditional_render", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glEndConditionalRenderNV", IsExtension=true)]
        public static void EndConditionalRenderNV()
        {
            _glEndConditionalRenderNV();
        }
        

        #endregion

        #region GL_NV_conservative_raster
        public const uint GL_CONSERVATIVE_RASTERIZATION_NV = (uint)0x9346;
        public const uint GL_SUBPIXEL_PRECISION_BIAS_X_BITS_NV = (uint)0x9347;
        public const uint GL_SUBPIXEL_PRECISION_BIAS_Y_BITS_NV = (uint)0x9348;
        public const uint GL_MAX_SUBPIXEL_PRECISION_BIAS_BITS_NV = (uint)0x9349;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glSubpixelPrecisionBiasNV(uint xbits, uint ybits);
        internal static glSubpixelPrecisionBiasNV _glSubpixelPrecisionBiasNV;

        [Version(Group="GL_NV_conservative_raster", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glSubpixelPrecisionBiasNV", IsExtension=true)]
        public static void SubpixelPrecisionBiasNV(uint xbits, uint ybits)
        {
            _glSubpixelPrecisionBiasNV(xbits, ybits);
        }
        

        #endregion

        #region GL_NV_conservative_raster_pre_snap_triangles
        public const uint GL_CONSERVATIVE_RASTER_MODE_NV = (uint)0x954D;
        public const uint GL_CONSERVATIVE_RASTER_MODE_POST_SNAP_NV = (uint)0x954E;
        public const uint GL_CONSERVATIVE_RASTER_MODE_PRE_SNAP_TRIANGLES_NV = (uint)0x954F;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glConservativeRasterParameteriNV(uint pname, int param);
        internal static glConservativeRasterParameteriNV _glConservativeRasterParameteriNV;

        [Version(Group="GL_NV_conservative_raster_pre_snap_triangles", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glConservativeRasterParameteriNV", IsExtension=true)]
        public static void ConservativeRasterParameteriNV(uint pname, int param)
        {
            _glConservativeRasterParameteriNV(pname, param);
        }
        

        #endregion

        #region GL_NV_fill_rectangle
        public const uint GL_FILL_RECTANGLE_NV = (uint)0x933C;


        #endregion

        #region GL_NV_fragment_coverage_to_color
        public const uint GL_FRAGMENT_COVERAGE_TO_COLOR_NV = (uint)0x92DD;
        public const uint GL_FRAGMENT_COVERAGE_COLOR_NV = (uint)0x92DE;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFragmentCoverageColorNV(uint color);
        internal static glFragmentCoverageColorNV _glFragmentCoverageColorNV;

        [Version(Group="GL_NV_fragment_coverage_to_color", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFragmentCoverageColorNV", IsExtension=true)]
        public static void FragmentCoverageColorNV(uint color)
        {
            _glFragmentCoverageColorNV(color);
        }
        

        #endregion

        #region GL_NV_framebuffer_mixed_samples
        public const uint GL_COVERAGE_MODULATION_TABLE_NV = (uint)0x9331;
        public const uint GL_COLOR_SAMPLES_NV = (uint)0x8E20;
        public const uint GL_DEPTH_SAMPLES_NV = (uint)0x932D;
        public const uint GL_STENCIL_SAMPLES_NV = (uint)0x932E;
        public const uint GL_MIXED_DEPTH_SAMPLES_SUPPORTED_NV = (uint)0x932F;
        public const uint GL_MIXED_STENCIL_SAMPLES_SUPPORTED_NV = (uint)0x9330;
        public const uint GL_COVERAGE_MODULATION_NV = (uint)0x9332;
        public const uint GL_COVERAGE_MODULATION_TABLE_SIZE_NV = (uint)0x9333;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCoverageModulationTableNV(int n, IntPtr v);
        internal static glCoverageModulationTableNV _glCoverageModulationTableNV;

        [Version(Group="GL_NV_framebuffer_mixed_samples", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCoverageModulationTableNV", IsExtension=true)]
        public static void CoverageModulationTableNV(int n, IntPtr v)
        {
            _glCoverageModulationTableNV(n, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetCoverageModulationTableNV(int bufsize, IntPtr v);
        internal static glGetCoverageModulationTableNV _glGetCoverageModulationTableNV;

        [Version(Group="GL_NV_framebuffer_mixed_samples", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetCoverageModulationTableNV", IsExtension=true)]
        public static void GetCoverageModulationTableNV(int bufsize, IntPtr v)
        {
            _glGetCoverageModulationTableNV(bufsize, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCoverageModulationNV(uint components);
        internal static glCoverageModulationNV _glCoverageModulationNV;

        [Version(Group="GL_NV_framebuffer_mixed_samples", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCoverageModulationNV", IsExtension=true)]
        public static void CoverageModulationNV(uint components)
        {
            _glCoverageModulationNV(components);
        }
        

        #endregion

        #region GL_NV_gpu_shader5
        public const uint GL_INT64_NV = (uint)0x140E;
        public const uint GL_UNSIGNED_INT64_NV = (uint)0x140F;
        public const uint GL_INT8_NV = (uint)0x8FE0;
        public const uint GL_INT8_VEC2_NV = (uint)0x8FE1;
        public const uint GL_INT8_VEC3_NV = (uint)0x8FE2;
        public const uint GL_INT8_VEC4_NV = (uint)0x8FE3;
        public const uint GL_INT16_NV = (uint)0x8FE4;
        public const uint GL_INT16_VEC2_NV = (uint)0x8FE5;
        public const uint GL_INT16_VEC3_NV = (uint)0x8FE6;
        public const uint GL_INT16_VEC4_NV = (uint)0x8FE7;
        public const uint GL_INT64_VEC2_NV = (uint)0x8FE9;
        public const uint GL_INT64_VEC3_NV = (uint)0x8FEA;
        public const uint GL_INT64_VEC4_NV = (uint)0x8FEB;
        public const uint GL_UNSIGNED_INT8_NV = (uint)0x8FEC;
        public const uint GL_UNSIGNED_INT8_VEC2_NV = (uint)0x8FED;
        public const uint GL_UNSIGNED_INT8_VEC3_NV = (uint)0x8FEE;
        public const uint GL_UNSIGNED_INT8_VEC4_NV = (uint)0x8FEF;
        public const uint GL_UNSIGNED_INT16_NV = (uint)0x8FF0;
        public const uint GL_UNSIGNED_INT16_VEC2_NV = (uint)0x8FF1;
        public const uint GL_UNSIGNED_INT16_VEC3_NV = (uint)0x8FF2;
        public const uint GL_UNSIGNED_INT16_VEC4_NV = (uint)0x8FF3;
        public const uint GL_UNSIGNED_INT64_VEC2_NV = (uint)0x8FF5;
        public const uint GL_UNSIGNED_INT64_VEC3_NV = (uint)0x8FF6;
        public const uint GL_UNSIGNED_INT64_VEC4_NV = (uint)0x8FF7;
        public const uint GL_FLOAT16_NV = (uint)0x8FF8;
        public const uint GL_FLOAT16_VEC2_NV = (uint)0x8FF9;
        public const uint GL_FLOAT16_VEC3_NV = (uint)0x8FFA;
        public const uint GL_FLOAT16_VEC4_NV = (uint)0x8FFB;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1i64NV(int location, long x);
        internal static glUniform1i64NV _glUniform1i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1i64NV", IsExtension=true)]
        public static void Uniform1i64NV(int location, long x)
        {
            _glUniform1i64NV(location, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2i64NV(int location, long x, long y);
        internal static glUniform2i64NV _glUniform2i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2i64NV", IsExtension=true)]
        public static void Uniform2i64NV(int location, long x, long y)
        {
            _glUniform2i64NV(location, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3i64NV(int location, long x, long y, long z);
        internal static glUniform3i64NV _glUniform3i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3i64NV", IsExtension=true)]
        public static void Uniform3i64NV(int location, long x, long y, long z)
        {
            _glUniform3i64NV(location, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4i64NV(int location, long x, long y, long z, long w);
        internal static glUniform4i64NV _glUniform4i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4i64NV", IsExtension=true)]
        public static void Uniform4i64NV(int location, long x, long y, long z, long w)
        {
            _glUniform4i64NV(location, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1i64vNV(int location, int count, long[] value);
        internal static glUniform1i64vNV _glUniform1i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1i64vNV", IsExtension=true)]
        public static void Uniform1i64vNV(int location, int count, long[] value)
        {
            _glUniform1i64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2i64vNV(int location, int count, long[] value);
        internal static glUniform2i64vNV _glUniform2i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2i64vNV", IsExtension=true)]
        public static void Uniform2i64vNV(int location, int count, long[] value)
        {
            _glUniform2i64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3i64vNV(int location, int count, long[] value);
        internal static glUniform3i64vNV _glUniform3i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3i64vNV", IsExtension=true)]
        public static void Uniform3i64vNV(int location, int count, long[] value)
        {
            _glUniform3i64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4i64vNV(int location, int count, long[] value);
        internal static glUniform4i64vNV _glUniform4i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4i64vNV", IsExtension=true)]
        public static void Uniform4i64vNV(int location, int count, long[] value)
        {
            _glUniform4i64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1ui64NV(int location, ulong x);
        internal static glUniform1ui64NV _glUniform1ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1ui64NV", IsExtension=true)]
        public static void Uniform1ui64NV(int location, ulong x)
        {
            _glUniform1ui64NV(location, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2ui64NV(int location, ulong x, ulong y);
        internal static glUniform2ui64NV _glUniform2ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2ui64NV", IsExtension=true)]
        public static void Uniform2ui64NV(int location, ulong x, ulong y)
        {
            _glUniform2ui64NV(location, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3ui64NV(int location, ulong x, ulong y, ulong z);
        internal static glUniform3ui64NV _glUniform3ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3ui64NV", IsExtension=true)]
        public static void Uniform3ui64NV(int location, ulong x, ulong y, ulong z)
        {
            _glUniform3ui64NV(location, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4ui64NV(int location, ulong x, ulong y, ulong z, ulong w);
        internal static glUniform4ui64NV _glUniform4ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4ui64NV", IsExtension=true)]
        public static void Uniform4ui64NV(int location, ulong x, ulong y, ulong z, ulong w)
        {
            _glUniform4ui64NV(location, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform1ui64vNV(int location, int count, ulong[] value);
        internal static glUniform1ui64vNV _glUniform1ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform1ui64vNV", IsExtension=true)]
        public static void Uniform1ui64vNV(int location, int count, ulong[] value)
        {
            _glUniform1ui64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform2ui64vNV(int location, int count, ulong[] value);
        internal static glUniform2ui64vNV _glUniform2ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform2ui64vNV", IsExtension=true)]
        public static void Uniform2ui64vNV(int location, int count, ulong[] value)
        {
            _glUniform2ui64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform3ui64vNV(int location, int count, ulong[] value);
        internal static glUniform3ui64vNV _glUniform3ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform3ui64vNV", IsExtension=true)]
        public static void Uniform3ui64vNV(int location, int count, ulong[] value)
        {
            _glUniform3ui64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glUniform4ui64vNV(int location, int count, ulong[] value);
        internal static glUniform4ui64vNV _glUniform4ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glUniform4ui64vNV", IsExtension=true)]
        public static void Uniform4ui64vNV(int location, int count, ulong[] value)
        {
            _glUniform4ui64vNV(location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetUniformi64vNV(uint program, int location, long[] @params);
        internal static glGetUniformi64vNV _glGetUniformi64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetUniformi64vNV", IsExtension=true)]
        public static void GetUniformi64vNV(uint program, int location, long[] @params)
        {
            _glGetUniformi64vNV(program, location, @params);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1i64NV(uint program, int location, long x);
        internal static glProgramUniform1i64NV _glProgramUniform1i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1i64NV", IsExtension=true)]
        public static void ProgramUniform1i64NV(uint program, int location, long x)
        {
            _glProgramUniform1i64NV(program, location, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2i64NV(uint program, int location, long x, long y);
        internal static glProgramUniform2i64NV _glProgramUniform2i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2i64NV", IsExtension=true)]
        public static void ProgramUniform2i64NV(uint program, int location, long x, long y)
        {
            _glProgramUniform2i64NV(program, location, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3i64NV(uint program, int location, long x, long y, long z);
        internal static glProgramUniform3i64NV _glProgramUniform3i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3i64NV", IsExtension=true)]
        public static void ProgramUniform3i64NV(uint program, int location, long x, long y, long z)
        {
            _glProgramUniform3i64NV(program, location, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4i64NV(uint program, int location, long x, long y, long z, long w);
        internal static glProgramUniform4i64NV _glProgramUniform4i64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4i64NV", IsExtension=true)]
        public static void ProgramUniform4i64NV(uint program, int location, long x, long y, long z, long w)
        {
            _glProgramUniform4i64NV(program, location, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1i64vNV(uint program, int location, int count, long[] value);
        internal static glProgramUniform1i64vNV _glProgramUniform1i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1i64vNV", IsExtension=true)]
        public static void ProgramUniform1i64vNV(uint program, int location, int count, long[] value)
        {
            _glProgramUniform1i64vNV(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2i64vNV(uint program, int location, int count, long[] value);
        internal static glProgramUniform2i64vNV _glProgramUniform2i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2i64vNV", IsExtension=true)]
        public static void ProgramUniform2i64vNV(uint program, int location, int count, long[] value)
        {
            _glProgramUniform2i64vNV(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3i64vNV(uint program, int location, int count, long[] value);
        internal static glProgramUniform3i64vNV _glProgramUniform3i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3i64vNV", IsExtension=true)]
        public static void ProgramUniform3i64vNV(uint program, int location, int count, long[] value)
        {
            _glProgramUniform3i64vNV(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4i64vNV(uint program, int location, int count, long[] value);
        internal static glProgramUniform4i64vNV _glProgramUniform4i64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4i64vNV", IsExtension=true)]
        public static void ProgramUniform4i64vNV(uint program, int location, int count, long[] value)
        {
            _glProgramUniform4i64vNV(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1ui64NV(uint program, int location, ulong x);
        internal static glProgramUniform1ui64NV _glProgramUniform1ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1ui64NV", IsExtension=true)]
        public static void ProgramUniform1ui64NV(uint program, int location, ulong x)
        {
            _glProgramUniform1ui64NV(program, location, x);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2ui64NV(uint program, int location, ulong x, ulong y);
        internal static glProgramUniform2ui64NV _glProgramUniform2ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2ui64NV", IsExtension=true)]
        public static void ProgramUniform2ui64NV(uint program, int location, ulong x, ulong y)
        {
            _glProgramUniform2ui64NV(program, location, x, y);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3ui64NV(uint program, int location, ulong x, ulong y, ulong z);
        internal static glProgramUniform3ui64NV _glProgramUniform3ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3ui64NV", IsExtension=true)]
        public static void ProgramUniform3ui64NV(uint program, int location, ulong x, ulong y, ulong z)
        {
            _glProgramUniform3ui64NV(program, location, x, y, z);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4ui64NV(uint program, int location, ulong x, ulong y, ulong z, ulong w);
        internal static glProgramUniform4ui64NV _glProgramUniform4ui64NV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4ui64NV", IsExtension=true)]
        public static void ProgramUniform4ui64NV(uint program, int location, ulong x, ulong y, ulong z, ulong w)
        {
            _glProgramUniform4ui64NV(program, location, x, y, z, w);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform1ui64vNV(uint program, int location, int count, ulong[] value);
        internal static glProgramUniform1ui64vNV _glProgramUniform1ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform1ui64vNV", IsExtension=true)]
        public static void ProgramUniform1ui64vNV(uint program, int location, int count, ulong[] value)
        {
            _glProgramUniform1ui64vNV(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform2ui64vNV(uint program, int location, int count, ulong[] value);
        internal static glProgramUniform2ui64vNV _glProgramUniform2ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform2ui64vNV", IsExtension=true)]
        public static void ProgramUniform2ui64vNV(uint program, int location, int count, ulong[] value)
        {
            _glProgramUniform2ui64vNV(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform3ui64vNV(uint program, int location, int count, ulong[] value);
        internal static glProgramUniform3ui64vNV _glProgramUniform3ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform3ui64vNV", IsExtension=true)]
        public static void ProgramUniform3ui64vNV(uint program, int location, int count, ulong[] value)
        {
            _glProgramUniform3ui64vNV(program, location, count, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramUniform4ui64vNV(uint program, int location, int count, ulong[] value);
        internal static glProgramUniform4ui64vNV _glProgramUniform4ui64vNV;

        [Version(Group="GL_NV_gpu_shader5", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramUniform4ui64vNV", IsExtension=true)]
        public static void ProgramUniform4ui64vNV(uint program, int location, int count, ulong[] value)
        {
            _glProgramUniform4ui64vNV(program, location, count, value);
        }
        

        #endregion

        #region GL_NV_internalformat_sample_query
        public const uint GL_MULTISAMPLES_NV = (uint)0x9371;
        public const uint GL_SUPERSAMPLE_SCALE_X_NV = (uint)0x9372;
        public const uint GL_SUPERSAMPLE_SCALE_Y_NV = (uint)0x9373;
        public const uint GL_CONFORMANT_NV = (uint)0x9374;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetInternalformatSampleivNV(uint target, uint internalformat, int samples, uint pname, int bufSize, int[] @params);
        internal static glGetInternalformatSampleivNV _glGetInternalformatSampleivNV;

        [Version(Group="GL_NV_internalformat_sample_query", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetInternalformatSampleivNV", IsExtension=true)]
        public static void GetInternalformatSampleivNV(uint target, uint internalformat, int samples, uint pname, int bufSize, int[] @params)
        {
            _glGetInternalformatSampleivNV(target, internalformat, samples, pname, bufSize, @params);
        }
        

        #endregion

        #region GL_NV_path_rendering
        public const uint GL_PATH_FORMAT_SVG_NV = (uint)0x9070;
        public const uint GL_PATH_FORMAT_PS_NV = (uint)0x9071;
        public const uint GL_STANDARD_FONT_NAME_NV = (uint)0x9072;
        public const uint GL_SYSTEM_FONT_NAME_NV = (uint)0x9073;
        public const uint GL_FILE_NAME_NV = (uint)0x9074;
        public const uint GL_PATH_STROKE_WIDTH_NV = (uint)0x9075;
        public const uint GL_PATH_END_CAPS_NV = (uint)0x9076;
        public const uint GL_PATH_INITIAL_END_CAP_NV = (uint)0x9077;
        public const uint GL_PATH_TERMINAL_END_CAP_NV = (uint)0x9078;
        public const uint GL_PATH_JOIN_STYLE_NV = (uint)0x9079;
        public const uint GL_PATH_MITER_LIMIT_NV = (uint)0x907A;
        public const uint GL_PATH_DASH_CAPS_NV = (uint)0x907B;
        public const uint GL_PATH_INITIAL_DASH_CAP_NV = (uint)0x907C;
        public const uint GL_PATH_TERMINAL_DASH_CAP_NV = (uint)0x907D;
        public const uint GL_PATH_DASH_OFFSET_NV = (uint)0x907E;
        public const uint GL_PATH_CLIENT_LENGTH_NV = (uint)0x907F;
        public const uint GL_PATH_FILL_MODE_NV = (uint)0x9080;
        public const uint GL_PATH_FILL_MASK_NV = (uint)0x9081;
        public const uint GL_PATH_FILL_COVER_MODE_NV = (uint)0x9082;
        public const uint GL_PATH_STROKE_COVER_MODE_NV = (uint)0x9083;
        public const uint GL_PATH_STROKE_MASK_NV = (uint)0x9084;
        public const uint GL_COUNT_UP_NV = (uint)0x9088;
        public const uint GL_COUNT_DOWN_NV = (uint)0x9089;
        public const uint GL_PATH_OBJECT_BOUNDING_BOX_NV = (uint)0x908A;
        public const uint GL_CONVEX_HULL_NV = (uint)0x908B;
        public const uint GL_BOUNDING_BOX_NV = (uint)0x908D;
        public const uint GL_TRANSLATE_X_NV = (uint)0x908E;
        public const uint GL_TRANSLATE_Y_NV = (uint)0x908F;
        public const uint GL_TRANSLATE_2D_NV = (uint)0x9090;
        public const uint GL_TRANSLATE_3D_NV = (uint)0x9091;
        public const uint GL_AFFINE_2D_NV = (uint)0x9092;
        public const uint GL_AFFINE_3D_NV = (uint)0x9094;
        public const uint GL_TRANSPOSE_AFFINE_2D_NV = (uint)0x9096;
        public const uint GL_TRANSPOSE_AFFINE_3D_NV = (uint)0x9098;
        public const uint GL_UTF8_NV = (uint)0x909A;
        public const uint GL_UTF16_NV = (uint)0x909B;
        public const uint GL_BOUNDING_BOX_OF_BOUNDING_BOXES_NV = (uint)0x909C;
        public const uint GL_PATH_COMMAND_COUNT_NV = (uint)0x909D;
        public const uint GL_PATH_COORD_COUNT_NV = (uint)0x909E;
        public const uint GL_PATH_DASH_ARRAY_COUNT_NV = (uint)0x909F;
        public const uint GL_PATH_COMPUTED_LENGTH_NV = (uint)0x90A0;
        public const uint GL_PATH_FILL_BOUNDING_BOX_NV = (uint)0x90A1;
        public const uint GL_PATH_STROKE_BOUNDING_BOX_NV = (uint)0x90A2;
        public const uint GL_SQUARE_NV = (uint)0x90A3;
        public const uint GL_ROUND_NV = (uint)0x90A4;
        public const uint GL_TRIANGULAR_NV = (uint)0x90A5;
        public const uint GL_BEVEL_NV = (uint)0x90A6;
        public const uint GL_MITER_REVERT_NV = (uint)0x90A7;
        public const uint GL_MITER_TRUNCATE_NV = (uint)0x90A8;
        public const uint GL_SKIP_MISSING_GLYPH_NV = (uint)0x90A9;
        public const uint GL_USE_MISSING_GLYPH_NV = (uint)0x90AA;
        public const uint GL_PATH_ERROR_POSITION_NV = (uint)0x90AB;
        public const uint GL_ACCUM_ADJACENT_PAIRS_NV = (uint)0x90AD;
        public const uint GL_ADJACENT_PAIRS_NV = (uint)0x90AE;
        public const uint GL_FIRST_TO_REST_NV = (uint)0x90AF;
        public const uint GL_PATH_GEN_MODE_NV = (uint)0x90B0;
        public const uint GL_PATH_GEN_COEFF_NV = (uint)0x90B1;
        public const uint GL_PATH_GEN_COMPONENTS_NV = (uint)0x90B3;
        public const uint GL_PATH_STENCIL_FUNC_NV = (uint)0x90B7;
        public const uint GL_PATH_STENCIL_REF_NV = (uint)0x90B8;
        public const uint GL_PATH_STENCIL_VALUE_MASK_NV = (uint)0x90B9;
        public const uint GL_PATH_STENCIL_DEPTH_OFFSET_FACTOR_NV = (uint)0x90BD;
        public const uint GL_PATH_STENCIL_DEPTH_OFFSET_UNITS_NV = (uint)0x90BE;
        public const uint GL_PATH_COVER_DEPTH_FUNC_NV = (uint)0x90BF;
        public const uint GL_PATH_DASH_OFFSET_RESET_NV = (uint)0x90B4;
        public const uint GL_MOVE_TO_RESETS_NV = (uint)0x90B5;
        public const uint GL_MOVE_TO_CONTINUES_NV = (uint)0x90B6;
        public const uint GL_CLOSE_PATH_NV = (uint)0x00;
        public const uint GL_MOVE_TO_NV = (uint)0x02;
        public const uint GL_RELATIVE_MOVE_TO_NV = (uint)0x03;
        public const uint GL_LINE_TO_NV = (uint)0x04;
        public const uint GL_RELATIVE_LINE_TO_NV = (uint)0x05;
        public const uint GL_HORIZONTAL_LINE_TO_NV = (uint)0x06;
        public const uint GL_RELATIVE_HORIZONTAL_LINE_TO_NV = (uint)0x07;
        public const uint GL_VERTICAL_LINE_TO_NV = (uint)0x08;
        public const uint GL_RELATIVE_VERTICAL_LINE_TO_NV = (uint)0x09;
        public const uint GL_QUADRATIC_CURVE_TO_NV = (uint)0x0A;
        public const uint GL_RELATIVE_QUADRATIC_CURVE_TO_NV = (uint)0x0B;
        public const uint GL_CUBIC_CURVE_TO_NV = (uint)0x0C;
        public const uint GL_RELATIVE_CUBIC_CURVE_TO_NV = (uint)0x0D;
        public const uint GL_SMOOTH_QUADRATIC_CURVE_TO_NV = (uint)0x0E;
        public const uint GL_RELATIVE_SMOOTH_QUADRATIC_CURVE_TO_NV = (uint)0x0F;
        public const uint GL_SMOOTH_CUBIC_CURVE_TO_NV = (uint)0x10;
        public const uint GL_RELATIVE_SMOOTH_CUBIC_CURVE_TO_NV = (uint)0x11;
        public const uint GL_SMALL_CCW_ARC_TO_NV = (uint)0x12;
        public const uint GL_RELATIVE_SMALL_CCW_ARC_TO_NV = (uint)0x13;
        public const uint GL_SMALL_CW_ARC_TO_NV = (uint)0x14;
        public const uint GL_RELATIVE_SMALL_CW_ARC_TO_NV = (uint)0x15;
        public const uint GL_LARGE_CCW_ARC_TO_NV = (uint)0x16;
        public const uint GL_RELATIVE_LARGE_CCW_ARC_TO_NV = (uint)0x17;
        public const uint GL_LARGE_CW_ARC_TO_NV = (uint)0x18;
        public const uint GL_RELATIVE_LARGE_CW_ARC_TO_NV = (uint)0x19;
        public const uint GL_RESTART_PATH_NV = (uint)0xF0;
        public const uint GL_DUP_FIRST_CUBIC_CURVE_TO_NV = (uint)0xF2;
        public const uint GL_DUP_LAST_CUBIC_CURVE_TO_NV = (uint)0xF4;
        public const uint GL_RECT_NV = (uint)0xF6;
        public const uint GL_CIRCULAR_CCW_ARC_TO_NV = (uint)0xF8;
        public const uint GL_CIRCULAR_CW_ARC_TO_NV = (uint)0xFA;
        public const uint GL_CIRCULAR_TANGENT_ARC_TO_NV = (uint)0xFC;
        public const uint GL_ARC_TO_NV = (uint)0xFE;
        public const uint GL_RELATIVE_ARC_TO_NV = (uint)0xFF;
        public const uint GL_BOLD_BIT_NV = (uint)0x01;
        public const uint GL_ITALIC_BIT_NV = (uint)0x02;
        public const uint GL_GLYPH_WIDTH_BIT_NV = (uint)0x01;
        public const uint GL_GLYPH_HEIGHT_BIT_NV = (uint)0x02;
        public const uint GL_GLYPH_HORIZONTAL_BEARING_X_BIT_NV = (uint)0x04;
        public const uint GL_GLYPH_HORIZONTAL_BEARING_Y_BIT_NV = (uint)0x08;
        public const uint GL_GLYPH_HORIZONTAL_BEARING_ADVANCE_BIT_NV = (uint)0x10;
        public const uint GL_GLYPH_VERTICAL_BEARING_X_BIT_NV = (uint)0x20;
        public const uint GL_GLYPH_VERTICAL_BEARING_Y_BIT_NV = (uint)0x40;
        public const uint GL_GLYPH_VERTICAL_BEARING_ADVANCE_BIT_NV = (uint)0x80;
        public const uint GL_GLYPH_HAS_KERNING_BIT_NV = (uint)0x100;
        public const uint GL_FONT_X_MIN_BOUNDS_BIT_NV = (uint)0x00010000;
        public const uint GL_FONT_Y_MIN_BOUNDS_BIT_NV = (uint)0x00020000;
        public const uint GL_FONT_X_MAX_BOUNDS_BIT_NV = (uint)0x00040000;
        public const uint GL_FONT_Y_MAX_BOUNDS_BIT_NV = (uint)0x00080000;
        public const uint GL_FONT_UNITS_PER_EM_BIT_NV = (uint)0x00100000;
        public const uint GL_FONT_ASCENDER_BIT_NV = (uint)0x00200000;
        public const uint GL_FONT_DESCENDER_BIT_NV = (uint)0x00400000;
        public const uint GL_FONT_HEIGHT_BIT_NV = (uint)0x00800000;
        public const uint GL_FONT_MAX_ADVANCE_WIDTH_BIT_NV = (uint)0x01000000;
        public const uint GL_FONT_MAX_ADVANCE_HEIGHT_BIT_NV = (uint)0x02000000;
        public const uint GL_FONT_UNDERLINE_POSITION_BIT_NV = (uint)0x04000000;
        public const uint GL_FONT_UNDERLINE_THICKNESS_BIT_NV = (uint)0x08000000;
        public const uint GL_FONT_HAS_KERNING_BIT_NV = (uint)0x10000000;
        public const uint GL_ROUNDED_RECT_NV = (uint)0xE8;
        public const uint GL_RELATIVE_ROUNDED_RECT_NV = (uint)0xE9;
        public const uint GL_ROUNDED_RECT2_NV = (uint)0xEA;
        public const uint GL_RELATIVE_ROUNDED_RECT2_NV = (uint)0xEB;
        public const uint GL_ROUNDED_RECT4_NV = (uint)0xEC;
        public const uint GL_RELATIVE_ROUNDED_RECT4_NV = (uint)0xED;
        public const uint GL_ROUNDED_RECT8_NV = (uint)0xEE;
        public const uint GL_RELATIVE_ROUNDED_RECT8_NV = (uint)0xEF;
        public const uint GL_RELATIVE_RECT_NV = (uint)0xF7;
        public const uint GL_FONT_GLYPHS_AVAILABLE_NV = (uint)0x9368;
        public const uint GL_FONT_TARGET_UNAVAILABLE_NV = (uint)0x9369;
        public const uint GL_FONT_UNAVAILABLE_NV = (uint)0x936A;
        public const uint GL_FONT_UNINTELLIGIBLE_NV = (uint)0x936B;
        public const uint GL_CONIC_CURVE_TO_NV = (uint)0x1A;
        public const uint GL_RELATIVE_CONIC_CURVE_TO_NV = (uint)0x1B;
        public const uint GL_FONT_NUM_GLYPH_INDICES_BIT_NV = (uint)0x20000000;
        public const uint GL_STANDARD_FONT_FORMAT_NV = (uint)0x936C;
        public const uint GL_PATH_PROJECTION_NV = (uint)0x1701;
        public const uint GL_PATH_MODELVIEW_NV = (uint)0x1700;
        public const uint GL_PATH_MODELVIEW_STACK_DEPTH_NV = (uint)0x0BA3;
        public const uint GL_PATH_MODELVIEW_MATRIX_NV = (uint)0x0BA6;
        public const uint GL_PATH_MAX_MODELVIEW_STACK_DEPTH_NV = (uint)0x0D36;
        public const uint GL_PATH_TRANSPOSE_MODELVIEW_MATRIX_NV = (uint)0x84E3;
        public const uint GL_PATH_PROJECTION_STACK_DEPTH_NV = (uint)0x0BA4;
        public const uint GL_PATH_PROJECTION_MATRIX_NV = (uint)0x0BA7;
        public const uint GL_PATH_MAX_PROJECTION_STACK_DEPTH_NV = (uint)0x0D38;
        public const uint GL_PATH_TRANSPOSE_PROJECTION_MATRIX_NV = (uint)0x84E4;
        public const uint GL_FRAGMENT_INPUT_NV = (uint)0x936D;

        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glGenPathsNV(int range);
        internal static glGenPathsNV _glGenPathsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGenPathsNV", IsExtension=true)]
        public static uint GenPathsNV(int range)
        {
            uint data = _glGenPathsNV(range);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glDeletePathsNV(uint path, int range);
        internal static glDeletePathsNV _glDeletePathsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glDeletePathsNV", IsExtension=true)]
        public static void DeletePathsNV(uint path, int range)
        {
            _glDeletePathsNV(path, range);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsPathNV(uint path);
        internal static glIsPathNV _glIsPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsPathNV", IsExtension=true)]
        public static bool IsPathNV(uint path)
        {
            bool data = _glIsPathNV(path);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathCommandsNV(uint path, int numCommands, string commands, int numCoords, uint coordType, IntPtr coords);
        internal static glPathCommandsNV _glPathCommandsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathCommandsNV", IsExtension=true)]
        public static void PathCommandsNV(uint path, int numCommands, string commands, int numCoords, uint coordType, IntPtr coords)
        {
            _glPathCommandsNV(path, numCommands, commands, numCoords, coordType, coords);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathCoordsNV(uint path, int numCoords, uint coordType, IntPtr coords);
        internal static glPathCoordsNV _glPathCoordsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathCoordsNV", IsExtension=true)]
        public static void PathCoordsNV(uint path, int numCoords, uint coordType, IntPtr coords)
        {
            _glPathCoordsNV(path, numCoords, coordType, coords);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathSubCommandsNV(uint path, int commandStart, int commandsToDelete, int numCommands, string commands, int numCoords, uint coordType, IntPtr coords);
        internal static glPathSubCommandsNV _glPathSubCommandsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathSubCommandsNV", IsExtension=true)]
        public static void PathSubCommandsNV(uint path, int commandStart, int commandsToDelete, int numCommands, string commands, int numCoords, uint coordType, IntPtr coords)
        {
            _glPathSubCommandsNV(path, commandStart, commandsToDelete, numCommands, commands, numCoords, coordType, coords);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathSubCoordsNV(uint path, int coordStart, int numCoords, uint coordType, IntPtr coords);
        internal static glPathSubCoordsNV _glPathSubCoordsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathSubCoordsNV", IsExtension=true)]
        public static void PathSubCoordsNV(uint path, int coordStart, int numCoords, uint coordType, IntPtr coords)
        {
            _glPathSubCoordsNV(path, coordStart, numCoords, coordType, coords);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathStringNV(uint path, uint format, int length, IntPtr pathString);
        internal static glPathStringNV _glPathStringNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathStringNV", IsExtension=true)]
        public static void PathStringNV(uint path, uint format, int length, IntPtr pathString)
        {
            _glPathStringNV(path, format, length, pathString);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathGlyphsNV(uint firstPathName, uint fontTarget, IntPtr fontName, uint fontStyle, int numGlyphs, uint type, IntPtr charcodes, uint handleMissingGlyphs, uint pathParameterTemplate, float emScale);
        internal static glPathGlyphsNV _glPathGlyphsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathGlyphsNV", IsExtension=true)]
        public static void PathGlyphsNV(uint firstPathName, uint fontTarget, IntPtr fontName, uint fontStyle, int numGlyphs, uint type, IntPtr charcodes, uint handleMissingGlyphs, uint pathParameterTemplate, float emScale)
        {
            _glPathGlyphsNV(firstPathName, fontTarget, fontName, fontStyle, numGlyphs, type, charcodes, handleMissingGlyphs, pathParameterTemplate, emScale);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathGlyphRangeNV(uint firstPathName, uint fontTarget, IntPtr fontName, uint fontStyle, uint firstGlyph, int numGlyphs, uint handleMissingGlyphs, uint pathParameterTemplate, float emScale);
        internal static glPathGlyphRangeNV _glPathGlyphRangeNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathGlyphRangeNV", IsExtension=true)]
        public static void PathGlyphRangeNV(uint firstPathName, uint fontTarget, IntPtr fontName, uint fontStyle, uint firstGlyph, int numGlyphs, uint handleMissingGlyphs, uint pathParameterTemplate, float emScale)
        {
            _glPathGlyphRangeNV(firstPathName, fontTarget, fontName, fontStyle, firstGlyph, numGlyphs, handleMissingGlyphs, pathParameterTemplate, emScale);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glWeightPathsNV(uint resultPath, int numPaths, uint[] paths, float[] weights);
        internal static glWeightPathsNV _glWeightPathsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glWeightPathsNV", IsExtension=true)]
        public static void WeightPathsNV(uint resultPath, int numPaths, uint[] paths, float[] weights)
        {
            _glWeightPathsNV(resultPath, numPaths, paths, weights);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCopyPathNV(uint resultPath, uint srcPath);
        internal static glCopyPathNV _glCopyPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCopyPathNV", IsExtension=true)]
        public static void CopyPathNV(uint resultPath, uint srcPath)
        {
            _glCopyPathNV(resultPath, srcPath);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glInterpolatePathsNV(uint resultPath, uint pathA, uint pathB, float weight);
        internal static glInterpolatePathsNV _glInterpolatePathsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glInterpolatePathsNV", IsExtension=true)]
        public static void InterpolatePathsNV(uint resultPath, uint pathA, uint pathB, float weight)
        {
            _glInterpolatePathsNV(resultPath, pathA, pathB, weight);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glTransformPathNV(uint resultPath, uint srcPath, uint transformType, float[] transformValues);
        internal static glTransformPathNV _glTransformPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glTransformPathNV", IsExtension=true)]
        public static void TransformPathNV(uint resultPath, uint srcPath, uint transformType, float[] transformValues)
        {
            _glTransformPathNV(resultPath, srcPath, transformType, transformValues);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathParameterivNV(uint path, uint pname, int[] value);
        internal static glPathParameterivNV _glPathParameterivNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathParameterivNV", IsExtension=true)]
        public static void PathParameterivNV(uint path, uint pname, int[] value)
        {
            _glPathParameterivNV(path, pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathParameteriNV(uint path, uint pname, int value);
        internal static glPathParameteriNV _glPathParameteriNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathParameteriNV", IsExtension=true)]
        public static void PathParameteriNV(uint path, uint pname, int value)
        {
            _glPathParameteriNV(path, pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathParameterfvNV(uint path, uint pname, float[] value);
        internal static glPathParameterfvNV _glPathParameterfvNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathParameterfvNV", IsExtension=true)]
        public static void PathParameterfvNV(uint path, uint pname, float[] value)
        {
            _glPathParameterfvNV(path, pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathParameterfNV(uint path, uint pname, float value);
        internal static glPathParameterfNV _glPathParameterfNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathParameterfNV", IsExtension=true)]
        public static void PathParameterfNV(uint path, uint pname, float value)
        {
            _glPathParameterfNV(path, pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathDashArrayNV(uint path, int dashCount, float[] dashArray);
        internal static glPathDashArrayNV _glPathDashArrayNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathDashArrayNV", IsExtension=true)]
        public static void PathDashArrayNV(uint path, int dashCount, float[] dashArray)
        {
            _glPathDashArrayNV(path, dashCount, dashArray);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathStencilFuncNV(uint func, int @ref, uint mask);
        internal static glPathStencilFuncNV _glPathStencilFuncNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathStencilFuncNV", IsExtension=true)]
        public static void PathStencilFuncNV(uint func, int @ref, uint mask)
        {
            _glPathStencilFuncNV(func, @ref, mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathStencilDepthOffsetNV(float factor, float units);
        internal static glPathStencilDepthOffsetNV _glPathStencilDepthOffsetNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathStencilDepthOffsetNV", IsExtension=true)]
        public static void PathStencilDepthOffsetNV(float factor, float units)
        {
            _glPathStencilDepthOffsetNV(factor, units);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilFillPathNV(uint path, uint fillMode, uint mask);
        internal static glStencilFillPathNV _glStencilFillPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilFillPathNV", IsExtension=true)]
        public static void StencilFillPathNV(uint path, uint fillMode, uint mask)
        {
            _glStencilFillPathNV(path, fillMode, mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilStrokePathNV(uint path, int reference, uint mask);
        internal static glStencilStrokePathNV _glStencilStrokePathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilStrokePathNV", IsExtension=true)]
        public static void StencilStrokePathNV(uint path, int reference, uint mask)
        {
            _glStencilStrokePathNV(path, reference, mask);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilFillPathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint fillMode, uint mask, uint transformType, float[] transformValues);
        internal static glStencilFillPathInstancedNV _glStencilFillPathInstancedNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilFillPathInstancedNV", IsExtension=true)]
        public static void StencilFillPathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint fillMode, uint mask, uint transformType, float[] transformValues)
        {
            _glStencilFillPathInstancedNV(numPaths, pathNameType, paths, pathBase, fillMode, mask, transformType, transformValues);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilStrokePathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, int reference, uint mask, uint transformType, float[] transformValues);
        internal static glStencilStrokePathInstancedNV _glStencilStrokePathInstancedNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilStrokePathInstancedNV", IsExtension=true)]
        public static void StencilStrokePathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, int reference, uint mask, uint transformType, float[] transformValues)
        {
            _glStencilStrokePathInstancedNV(numPaths, pathNameType, paths, pathBase, reference, mask, transformType, transformValues);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glPathCoverDepthFuncNV(uint func);
        internal static glPathCoverDepthFuncNV _glPathCoverDepthFuncNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathCoverDepthFuncNV", IsExtension=true)]
        public static void PathCoverDepthFuncNV(uint func)
        {
            _glPathCoverDepthFuncNV(func);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCoverFillPathNV(uint path, uint coverMode);
        internal static glCoverFillPathNV _glCoverFillPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCoverFillPathNV", IsExtension=true)]
        public static void CoverFillPathNV(uint path, uint coverMode)
        {
            _glCoverFillPathNV(path, coverMode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCoverStrokePathNV(uint path, uint coverMode);
        internal static glCoverStrokePathNV _glCoverStrokePathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCoverStrokePathNV", IsExtension=true)]
        public static void CoverStrokePathNV(uint path, uint coverMode)
        {
            _glCoverStrokePathNV(path, coverMode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCoverFillPathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint coverMode, uint transformType, float[] transformValues);
        internal static glCoverFillPathInstancedNV _glCoverFillPathInstancedNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCoverFillPathInstancedNV", IsExtension=true)]
        public static void CoverFillPathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint coverMode, uint transformType, float[] transformValues)
        {
            _glCoverFillPathInstancedNV(numPaths, pathNameType, paths, pathBase, coverMode, transformType, transformValues);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glCoverStrokePathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint coverMode, uint transformType, float[] transformValues);
        internal static glCoverStrokePathInstancedNV _glCoverStrokePathInstancedNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glCoverStrokePathInstancedNV", IsExtension=true)]
        public static void CoverStrokePathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint coverMode, uint transformType, float[] transformValues)
        {
            _glCoverStrokePathInstancedNV(numPaths, pathNameType, paths, pathBase, coverMode, transformType, transformValues);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathParameterivNV(uint path, uint pname, int[] value);
        internal static glGetPathParameterivNV _glGetPathParameterivNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathParameterivNV", IsExtension=true)]
        public static void GetPathParameterivNV(uint path, uint pname, int[] value)
        {
            _glGetPathParameterivNV(path, pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathParameterfvNV(uint path, uint pname, float[] value);
        internal static glGetPathParameterfvNV _glGetPathParameterfvNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathParameterfvNV", IsExtension=true)]
        public static void GetPathParameterfvNV(uint path, uint pname, float[] value)
        {
            _glGetPathParameterfvNV(path, pname, value);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathCommandsNV(uint path, byte[] commands);
        internal static glGetPathCommandsNV _glGetPathCommandsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathCommandsNV", IsExtension=true)]
        public static void GetPathCommandsNV(uint path, byte[] commands)
        {
            _glGetPathCommandsNV(path, commands);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathCoordsNV(uint path, float[] coords);
        internal static glGetPathCoordsNV _glGetPathCoordsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathCoordsNV", IsExtension=true)]
        public static void GetPathCoordsNV(uint path, float[] coords)
        {
            _glGetPathCoordsNV(path, coords);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathDashArrayNV(uint path, float[] dashArray);
        internal static glGetPathDashArrayNV _glGetPathDashArrayNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathDashArrayNV", IsExtension=true)]
        public static void GetPathDashArrayNV(uint path, float[] dashArray)
        {
            _glGetPathDashArrayNV(path, dashArray);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathMetricsNV(uint metricQueryMask, int numPaths, uint pathNameType, IntPtr paths, uint pathBase, int stride, float[] metrics);
        internal static glGetPathMetricsNV _glGetPathMetricsNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathMetricsNV", IsExtension=true)]
        public static void GetPathMetricsNV(uint metricQueryMask, int numPaths, uint pathNameType, IntPtr paths, uint pathBase, int stride, float[] metrics)
        {
            _glGetPathMetricsNV(metricQueryMask, numPaths, pathNameType, paths, pathBase, stride, metrics);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathMetricRangeNV(uint metricQueryMask, uint firstPathName, int numPaths, int stride, float[] metrics);
        internal static glGetPathMetricRangeNV _glGetPathMetricRangeNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathMetricRangeNV", IsExtension=true)]
        public static void GetPathMetricRangeNV(uint metricQueryMask, uint firstPathName, int numPaths, int stride, float[] metrics)
        {
            _glGetPathMetricRangeNV(metricQueryMask, firstPathName, numPaths, stride, metrics);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetPathSpacingNV(uint pathListMode, int numPaths, uint pathNameType, IntPtr paths, uint pathBase, float advanceScale, float kerningScale, uint transformType, float[] returnedSpacing);
        internal static glGetPathSpacingNV _glGetPathSpacingNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathSpacingNV", IsExtension=true)]
        public static void GetPathSpacingNV(uint pathListMode, int numPaths, uint pathNameType, IntPtr paths, uint pathBase, float advanceScale, float kerningScale, uint transformType, float[] returnedSpacing)
        {
            _glGetPathSpacingNV(pathListMode, numPaths, pathNameType, paths, pathBase, advanceScale, kerningScale, transformType, returnedSpacing);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsPointInFillPathNV(uint path, uint mask, float x, float y);
        internal static glIsPointInFillPathNV _glIsPointInFillPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsPointInFillPathNV", IsExtension=true)]
        public static bool IsPointInFillPathNV(uint path, uint mask, float x, float y)
        {
            bool data = _glIsPointInFillPathNV(path, mask, x, y);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glIsPointInStrokePathNV(uint path, float x, float y);
        internal static glIsPointInStrokePathNV _glIsPointInStrokePathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glIsPointInStrokePathNV", IsExtension=true)]
        public static bool IsPointInStrokePathNV(uint path, float x, float y)
        {
            bool data = _glIsPointInStrokePathNV(path, x, y);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate float glGetPathLengthNV(uint path, int startSegment, int numSegments);
        internal static glGetPathLengthNV _glGetPathLengthNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetPathLengthNV", IsExtension=true)]
        public static float GetPathLengthNV(uint path, int startSegment, int numSegments)
        {
            float data = _glGetPathLengthNV(path, startSegment, numSegments);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate bool glPointAlongPathNV(uint path, int startSegment, int numSegments, float distance, IntPtr x, IntPtr y, IntPtr tangentX, IntPtr tangentY);
        internal static glPointAlongPathNV _glPointAlongPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPointAlongPathNV", IsExtension=true)]
        public static bool PointAlongPathNV(uint path, int startSegment, int numSegments, float distance, IntPtr x, IntPtr y, IntPtr tangentX, IntPtr tangentY)
        {
            bool data = _glPointAlongPathNV(path, startSegment, numSegments, distance, x, y, tangentX, tangentY);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMatrixLoad3x2fNV(uint matrixMode, IntPtr m);
        internal static glMatrixLoad3x2fNV _glMatrixLoad3x2fNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMatrixLoad3x2fNV", IsExtension=true)]
        public static void MatrixLoad3x2fNV(uint matrixMode, IntPtr m)
        {
            _glMatrixLoad3x2fNV(matrixMode, m);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMatrixLoad3x3fNV(uint matrixMode, IntPtr m);
        internal static glMatrixLoad3x3fNV _glMatrixLoad3x3fNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMatrixLoad3x3fNV", IsExtension=true)]
        public static void MatrixLoad3x3fNV(uint matrixMode, IntPtr m)
        {
            _glMatrixLoad3x3fNV(matrixMode, m);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMatrixLoadTranspose3x3fNV(uint matrixMode, IntPtr m);
        internal static glMatrixLoadTranspose3x3fNV _glMatrixLoadTranspose3x3fNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMatrixLoadTranspose3x3fNV", IsExtension=true)]
        public static void MatrixLoadTranspose3x3fNV(uint matrixMode, IntPtr m)
        {
            _glMatrixLoadTranspose3x3fNV(matrixMode, m);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMatrixMult3x2fNV(uint matrixMode, IntPtr m);
        internal static glMatrixMult3x2fNV _glMatrixMult3x2fNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMatrixMult3x2fNV", IsExtension=true)]
        public static void MatrixMult3x2fNV(uint matrixMode, IntPtr m)
        {
            _glMatrixMult3x2fNV(matrixMode, m);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMatrixMult3x3fNV(uint matrixMode, IntPtr m);
        internal static glMatrixMult3x3fNV _glMatrixMult3x3fNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMatrixMult3x3fNV", IsExtension=true)]
        public static void MatrixMult3x3fNV(uint matrixMode, IntPtr m)
        {
            _glMatrixMult3x3fNV(matrixMode, m);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glMatrixMultTranspose3x3fNV(uint matrixMode, IntPtr m);
        internal static glMatrixMultTranspose3x3fNV _glMatrixMultTranspose3x3fNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glMatrixMultTranspose3x3fNV", IsExtension=true)]
        public static void MatrixMultTranspose3x3fNV(uint matrixMode, IntPtr m)
        {
            _glMatrixMultTranspose3x3fNV(matrixMode, m);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilThenCoverFillPathNV(uint path, uint fillMode, uint mask, uint coverMode);
        internal static glStencilThenCoverFillPathNV _glStencilThenCoverFillPathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilThenCoverFillPathNV", IsExtension=true)]
        public static void StencilThenCoverFillPathNV(uint path, uint fillMode, uint mask, uint coverMode)
        {
            _glStencilThenCoverFillPathNV(path, fillMode, mask, coverMode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilThenCoverStrokePathNV(uint path, int reference, uint mask, uint coverMode);
        internal static glStencilThenCoverStrokePathNV _glStencilThenCoverStrokePathNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilThenCoverStrokePathNV", IsExtension=true)]
        public static void StencilThenCoverStrokePathNV(uint path, int reference, uint mask, uint coverMode)
        {
            _glStencilThenCoverStrokePathNV(path, reference, mask, coverMode);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilThenCoverFillPathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint fillMode, uint mask, uint coverMode, uint transformType, IntPtr transformValues);
        internal static glStencilThenCoverFillPathInstancedNV _glStencilThenCoverFillPathInstancedNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilThenCoverFillPathInstancedNV", IsExtension=true)]
        public static void StencilThenCoverFillPathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, uint fillMode, uint mask, uint coverMode, uint transformType, IntPtr transformValues)
        {
            _glStencilThenCoverFillPathInstancedNV(numPaths, pathNameType, paths, pathBase, fillMode, mask, coverMode, transformType, transformValues);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glStencilThenCoverStrokePathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, int reference, uint mask, uint coverMode, uint transformType, IntPtr transformValues);
        internal static glStencilThenCoverStrokePathInstancedNV _glStencilThenCoverStrokePathInstancedNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glStencilThenCoverStrokePathInstancedNV", IsExtension=true)]
        public static void StencilThenCoverStrokePathInstancedNV(int numPaths, uint pathNameType, IntPtr paths, uint pathBase, int reference, uint mask, uint coverMode, uint transformType, IntPtr transformValues)
        {
            _glStencilThenCoverStrokePathInstancedNV(numPaths, pathNameType, paths, pathBase, reference, mask, coverMode, transformType, transformValues);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glPathGlyphIndexRangeNV(uint fontTarget, IntPtr fontName, uint fontStyle, uint pathParameterTemplate, float emScale, uint baseAndCount);
        internal static glPathGlyphIndexRangeNV _glPathGlyphIndexRangeNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathGlyphIndexRangeNV", IsExtension=true)]
        public static uint PathGlyphIndexRangeNV(uint fontTarget, IntPtr fontName, uint fontStyle, uint pathParameterTemplate, float emScale, uint baseAndCount)
        {
            uint data = _glPathGlyphIndexRangeNV(fontTarget, fontName, fontStyle, pathParameterTemplate, emScale, baseAndCount);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glPathGlyphIndexArrayNV(uint firstPathName, uint fontTarget, IntPtr fontName, uint fontStyle, uint firstGlyphIndex, int numGlyphs, uint pathParameterTemplate, float emScale);
        internal static glPathGlyphIndexArrayNV _glPathGlyphIndexArrayNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathGlyphIndexArrayNV", IsExtension=true)]
        public static uint PathGlyphIndexArrayNV(uint firstPathName, uint fontTarget, IntPtr fontName, uint fontStyle, uint firstGlyphIndex, int numGlyphs, uint pathParameterTemplate, float emScale)
        {
            uint data = _glPathGlyphIndexArrayNV(firstPathName, fontTarget, fontName, fontStyle, firstGlyphIndex, numGlyphs, pathParameterTemplate, emScale);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate uint glPathMemoryGlyphIndexArrayNV(uint firstPathName, uint fontTarget, IntPtr fontSize, IntPtr fontData, int faceIndex, uint firstGlyphIndex, int numGlyphs, uint pathParameterTemplate, float emScale);
        internal static glPathMemoryGlyphIndexArrayNV _glPathMemoryGlyphIndexArrayNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glPathMemoryGlyphIndexArrayNV", IsExtension=true)]
        public static uint PathMemoryGlyphIndexArrayNV(uint firstPathName, uint fontTarget, IntPtr fontSize, IntPtr fontData, int faceIndex, uint firstGlyphIndex, int numGlyphs, uint pathParameterTemplate, float emScale)
        {
            uint data = _glPathMemoryGlyphIndexArrayNV(firstPathName, fontTarget, fontSize, fontData, faceIndex, firstGlyphIndex, numGlyphs, pathParameterTemplate, emScale);
            return data;
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glProgramPathFragmentInputGenNV(uint program, int location, uint genMode, int components, IntPtr coeffs);
        internal static glProgramPathFragmentInputGenNV _glProgramPathFragmentInputGenNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glProgramPathFragmentInputGenNV", IsExtension=true)]
        public static void ProgramPathFragmentInputGenNV(uint program, int location, uint genMode, int components, IntPtr coeffs)
        {
            _glProgramPathFragmentInputGenNV(program, location, genMode, components, coeffs);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glGetProgramResourcefvNV(uint program, uint programInterface, uint index, int propCount, IntPtr props, int bufSize, IntPtr length, IntPtr @params);
        internal static glGetProgramResourcefvNV _glGetProgramResourcefvNV;

        [Version(Group="GL_NV_path_rendering", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glGetProgramResourcefvNV", IsExtension=true)]
        public static void GetProgramResourcefvNV(uint program, uint programInterface, uint index, int propCount, IntPtr props, int bufSize, IntPtr length, IntPtr @params)
        {
            _glGetProgramResourcefvNV(program, programInterface, index, propCount, props, bufSize, length, @params);
        }
        

        #endregion

        #region GL_NV_path_rendering_shared_edge
        public const uint GL_SHARED_EDGE_NV = (uint)0xC0;


        #endregion

        #region GL_NV_sample_locations
        public const uint GL_SAMPLE_LOCATION_SUBPIXEL_BITS_NV = (uint)0x933D;
        public const uint GL_SAMPLE_LOCATION_PIXEL_GRID_WIDTH_NV = (uint)0x933E;
        public const uint GL_SAMPLE_LOCATION_PIXEL_GRID_HEIGHT_NV = (uint)0x933F;
        public const uint GL_PROGRAMMABLE_SAMPLE_LOCATION_TABLE_SIZE_NV = (uint)0x9340;
        public const uint GL_SAMPLE_LOCATION_NV = (uint)0x8E50;
        public const uint GL_PROGRAMMABLE_SAMPLE_LOCATION_NV = (uint)0x9341;
        public const uint GL_FRAMEBUFFER_PROGRAMMABLE_SAMPLE_LOCATIONS_NV = (uint)0x9342;
        public const uint GL_FRAMEBUFFER_SAMPLE_LOCATION_PIXEL_GRID_NV = (uint)0x9343;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferSampleLocationsfvNV(uint target, uint start, int count, IntPtr v);
        internal static glFramebufferSampleLocationsfvNV _glFramebufferSampleLocationsfvNV;

        [Version(Group="GL_NV_sample_locations", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferSampleLocationsfvNV", IsExtension=true)]
        public static void FramebufferSampleLocationsfvNV(uint target, uint start, int count, IntPtr v)
        {
            _glFramebufferSampleLocationsfvNV(target, start, count, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glNamedFramebufferSampleLocationsfvNV(uint framebuffer, uint start, int count, IntPtr v);
        internal static glNamedFramebufferSampleLocationsfvNV _glNamedFramebufferSampleLocationsfvNV;

        [Version(Group="GL_NV_sample_locations", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glNamedFramebufferSampleLocationsfvNV", IsExtension=true)]
        public static void NamedFramebufferSampleLocationsfvNV(uint framebuffer, uint start, int count, IntPtr v)
        {
            _glNamedFramebufferSampleLocationsfvNV(framebuffer, start, count, v);
        }
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate void glResolveDepthValuesNV();
        internal static glResolveDepthValuesNV _glResolveDepthValuesNV;

        [Version(Group="GL_NV_sample_locations", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glResolveDepthValuesNV", IsExtension=true)]
        public static void ResolveDepthValuesNV()
        {
            _glResolveDepthValuesNV();
        }
        

        #endregion

        #region GL_NV_viewport_swizzle
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_X_NV = (uint)0x9350;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_X_NV = (uint)0x9351;
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_Y_NV = (uint)0x9352;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_Y_NV = (uint)0x9353;
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_Z_NV = (uint)0x9354;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_Z_NV = (uint)0x9355;
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_W_NV = (uint)0x9356;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_W_NV = (uint)0x9357;
        public const uint GL_VIEWPORT_SWIZZLE_X_NV = (uint)0x9358;
        public const uint GL_VIEWPORT_SWIZZLE_Y_NV = (uint)0x9359;
        public const uint GL_VIEWPORT_SWIZZLE_Z_NV = (uint)0x935A;
        public const uint GL_VIEWPORT_SWIZZLE_W_NV = (uint)0x935B;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glViewportSwizzleNV(uint index, uint swizzlex, uint swizzley, uint swizzlez, uint swizzlew);
        internal static glViewportSwizzleNV _glViewportSwizzleNV;

        [Version(Group="GL_NV_viewport_swizzle", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glViewportSwizzleNV", IsExtension=true)]
        public static void ViewportSwizzleNV(uint index, uint swizzlex, uint swizzley, uint swizzlez, uint swizzlew)
        {
            _glViewportSwizzleNV(index, swizzlex, swizzley, swizzlez, swizzlew);
        }
        

        #endregion

        #region GL_OVR_multiview
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_NUM_VIEWS_OVR = (uint)0x9630;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_BASE_VIEW_INDEX_OVR = (uint)0x9632;
        public const uint GL_MAX_VIEWS_OVR = (uint)0x9631;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_VIEW_TARGETS_OVR = (uint)0x9633;

        [SuppressUnmanagedCodeSecurity]
        internal delegate void glFramebufferTextureMultiviewOVR(uint target, uint attachment, uint texture, int level, int baseViewIndex, int numViews);
        internal static glFramebufferTextureMultiviewOVR _glFramebufferTextureMultiviewOVR;

        [Version(Group="GL_OVR_multiview", Version = "", Profile="all", DeprecatedVersion="", DeprecatedProfile="", EntryPoint="glFramebufferTextureMultiviewOVR", IsExtension=true)]
        public static void FramebufferTextureMultiviewOVR(uint target, uint attachment, uint texture, int level, int baseViewIndex, int numViews)
        {
            _glFramebufferTextureMultiviewOVR(target, attachment, texture, level, baseViewIndex, numViews);
        }
        

        #endregion


        #endregion

        /// <summary>
        /// Init library for everything defined.
        /// </summary>
        public static void Load()
        {
            LoadLib();

            _glCullFace = LoadFunction<glCullFace>();
            _glFrontFace = LoadFunction<glFrontFace>();
            _glHint = LoadFunction<glHint>();
            _glLineWidth = LoadFunction<glLineWidth>();
            _glPointSize = LoadFunction<glPointSize>();
            _glPolygonMode = LoadFunction<glPolygonMode>();
            _glScissor = LoadFunction<glScissor>();
            _glTexParameterf = LoadFunction<glTexParameterf>();
            _glTexParameterfv = LoadFunction<glTexParameterfv>();
            _glTexParameteri = LoadFunction<glTexParameteri>();
            _glTexParameteriv = LoadFunction<glTexParameteriv>();
            _glTexImage1D = LoadFunction<glTexImage1D>();
            _glTexImage2D = LoadFunction<glTexImage2D>();
            _glDrawBuffer = LoadFunction<glDrawBuffer>();
            _glClear = LoadFunction<glClear>();
            _glClearColor = LoadFunction<glClearColor>();
            _glClearStencil = LoadFunction<glClearStencil>();
            _glClearDepth = LoadFunction<glClearDepth>();
            _glStencilMask = LoadFunction<glStencilMask>();
            _glColorMask = LoadFunction<glColorMask>();
            _glDepthMask = LoadFunction<glDepthMask>();
            _glDisable = LoadFunction<glDisable>();
            _glEnable = LoadFunction<glEnable>();
            _glFinish = LoadFunction<glFinish>();
            _glFlush = LoadFunction<glFlush>();
            _glBlendFunc = LoadFunction<glBlendFunc>();
            _glLogicOp = LoadFunction<glLogicOp>();
            _glStencilFunc = LoadFunction<glStencilFunc>();
            _glStencilOp = LoadFunction<glStencilOp>();
            _glDepthFunc = LoadFunction<glDepthFunc>();
            _glPixelStoref = LoadFunction<glPixelStoref>();
            _glPixelStorei = LoadFunction<glPixelStorei>();
            _glReadBuffer = LoadFunction<glReadBuffer>();
            _glReadPixels = LoadFunction<glReadPixels>();
            _glGetBooleanv = LoadFunction<glGetBooleanv>();
            _glGetDoublev = LoadFunction<glGetDoublev>();
            _glGetError = LoadFunction<glGetError>();
            _glGetFloatv = LoadFunction<glGetFloatv>();
            _glGetIntegerv = LoadFunction<glGetIntegerv>();
            _glGetString = LoadFunction<glGetString>();
            _glGetTexImage = LoadFunction<glGetTexImage>();
            _glGetTexParameterfv = LoadFunction<glGetTexParameterfv>();
            _glGetTexParameteriv = LoadFunction<glGetTexParameteriv>();
            _glGetTexLevelParameterfv = LoadFunction<glGetTexLevelParameterfv>();
            _glGetTexLevelParameteriv = LoadFunction<glGetTexLevelParameteriv>();
            _glIsEnabled = LoadFunction<glIsEnabled>();
            _glDepthRange = LoadFunction<glDepthRange>();
            _glViewport = LoadFunction<glViewport>();
            _glDrawArrays = LoadFunction<glDrawArrays>();
            _glDrawElements = LoadFunction<glDrawElements>();
            _glPolygonOffset = LoadFunction<glPolygonOffset>();
            _glCopyTexImage1D = LoadFunction<glCopyTexImage1D>();
            _glCopyTexImage2D = LoadFunction<glCopyTexImage2D>();
            _glCopyTexSubImage1D = LoadFunction<glCopyTexSubImage1D>();
            _glCopyTexSubImage2D = LoadFunction<glCopyTexSubImage2D>();
            _glTexSubImage1D = LoadFunction<glTexSubImage1D>();
            _glTexSubImage2D = LoadFunction<glTexSubImage2D>();
            _glBindTexture = LoadFunction<glBindTexture>();
            _glDeleteTextures = LoadFunction<glDeleteTextures>();
            _glGenTextures = LoadFunction<glGenTextures>();
            _glIsTexture = LoadFunction<glIsTexture>();
            _glDrawRangeElements = LoadFunction<glDrawRangeElements>();
            _glTexImage3D = LoadFunction<glTexImage3D>();
            _glTexSubImage3D = LoadFunction<glTexSubImage3D>();
            _glCopyTexSubImage3D = LoadFunction<glCopyTexSubImage3D>();
            _glActiveTexture = LoadFunction<glActiveTexture>();
            _glSampleCoverage = LoadFunction<glSampleCoverage>();
            _glCompressedTexImage3D = LoadFunction<glCompressedTexImage3D>();
            _glCompressedTexImage2D = LoadFunction<glCompressedTexImage2D>();
            _glCompressedTexImage1D = LoadFunction<glCompressedTexImage1D>();
            _glCompressedTexSubImage3D = LoadFunction<glCompressedTexSubImage3D>();
            _glCompressedTexSubImage2D = LoadFunction<glCompressedTexSubImage2D>();
            _glCompressedTexSubImage1D = LoadFunction<glCompressedTexSubImage1D>();
            _glGetCompressedTexImage = LoadFunction<glGetCompressedTexImage>();
            _glBlendFuncSeparate = LoadFunction<glBlendFuncSeparate>();
            _glMultiDrawArrays = LoadFunction<glMultiDrawArrays>();
            _glMultiDrawElements = LoadFunction<glMultiDrawElements>();
            _glPointParameterf = LoadFunction<glPointParameterf>();
            _glPointParameterfv = LoadFunction<glPointParameterfv>();
            _glPointParameteri = LoadFunction<glPointParameteri>();
            _glPointParameteriv = LoadFunction<glPointParameteriv>();
            _glBlendColor = LoadFunction<glBlendColor>();
            _glBlendEquation = LoadFunction<glBlendEquation>();
            _glGenQueries = LoadFunction<glGenQueries>();
            _glDeleteQueries = LoadFunction<glDeleteQueries>();
            _glIsQuery = LoadFunction<glIsQuery>();
            _glBeginQuery = LoadFunction<glBeginQuery>();
            _glEndQuery = LoadFunction<glEndQuery>();
            _glGetQueryiv = LoadFunction<glGetQueryiv>();
            _glGetQueryObjectiv = LoadFunction<glGetQueryObjectiv>();
            _glGetQueryObjectuiv = LoadFunction<glGetQueryObjectuiv>();
            _glBindBuffer = LoadFunction<glBindBuffer>();
            _glDeleteBuffers = LoadFunction<glDeleteBuffers>();
            _glGenBuffers = LoadFunction<glGenBuffers>();
            _glIsBuffer = LoadFunction<glIsBuffer>();
            _glBufferData = LoadFunction<glBufferData>();
            _glBufferSubData = LoadFunction<glBufferSubData>();
            _glGetBufferSubData = LoadFunction<glGetBufferSubData>();
            _glMapBuffer = LoadFunction<glMapBuffer>();
            _glUnmapBuffer = LoadFunction<glUnmapBuffer>();
            _glGetBufferParameteriv = LoadFunction<glGetBufferParameteriv>();
            _glGetBufferPointerv = LoadFunction<glGetBufferPointerv>();
            _glBlendEquationSeparate = LoadFunction<glBlendEquationSeparate>();
            _glDrawBuffers = LoadFunction<glDrawBuffers>();
            _glStencilOpSeparate = LoadFunction<glStencilOpSeparate>();
            _glStencilFuncSeparate = LoadFunction<glStencilFuncSeparate>();
            _glStencilMaskSeparate = LoadFunction<glStencilMaskSeparate>();
            _glAttachShader = LoadFunction<glAttachShader>();
            _glBindAttribLocation = LoadFunction<glBindAttribLocation>();
            _glCompileShader = LoadFunction<glCompileShader>();
            _glCreateProgram = LoadFunction<glCreateProgram>();
            _glCreateShader = LoadFunction<glCreateShader>();
            _glDeleteProgram = LoadFunction<glDeleteProgram>();
            _glDeleteShader = LoadFunction<glDeleteShader>();
            _glDetachShader = LoadFunction<glDetachShader>();
            _glDisableVertexAttribArray = LoadFunction<glDisableVertexAttribArray>();
            _glEnableVertexAttribArray = LoadFunction<glEnableVertexAttribArray>();
            _glGetActiveAttrib = LoadFunction<glGetActiveAttrib>();
            _glGetActiveUniform = LoadFunction<glGetActiveUniform>();
            _glGetAttachedShaders = LoadFunction<glGetAttachedShaders>();
            _glGetAttribLocation = LoadFunction<glGetAttribLocation>();
            _glGetProgramiv = LoadFunction<glGetProgramiv>();
            _glGetProgramInfoLog = LoadFunction<glGetProgramInfoLog>();
            _glGetShaderiv = LoadFunction<glGetShaderiv>();
            _glGetShaderInfoLog = LoadFunction<glGetShaderInfoLog>();
            _glGetShaderSource = LoadFunction<glGetShaderSource>();
            _glGetUniformLocation = LoadFunction<glGetUniformLocation>();
            _glGetUniformfv = LoadFunction<glGetUniformfv>();
            _glGetUniformiv = LoadFunction<glGetUniformiv>();
            _glGetVertexAttribdv = LoadFunction<glGetVertexAttribdv>();
            _glGetVertexAttribfv = LoadFunction<glGetVertexAttribfv>();
            _glGetVertexAttribiv = LoadFunction<glGetVertexAttribiv>();
            _glGetVertexAttribPointerv = LoadFunction<glGetVertexAttribPointerv>();
            _glIsProgram = LoadFunction<glIsProgram>();
            _glIsShader = LoadFunction<glIsShader>();
            _glLinkProgram = LoadFunction<glLinkProgram>();
            _glShaderSource = LoadFunction<glShaderSource>();
            _glUseProgram = LoadFunction<glUseProgram>();
            _glUniform1f = LoadFunction<glUniform1f>();
            _glUniform2f = LoadFunction<glUniform2f>();
            _glUniform3f = LoadFunction<glUniform3f>();
            _glUniform4f = LoadFunction<glUniform4f>();
            _glUniform1i = LoadFunction<glUniform1i>();
            _glUniform2i = LoadFunction<glUniform2i>();
            _glUniform3i = LoadFunction<glUniform3i>();
            _glUniform4i = LoadFunction<glUniform4i>();
            _glUniform1fv = LoadFunction<glUniform1fv>();
            _glUniform2fv = LoadFunction<glUniform2fv>();
            _glUniform3fv = LoadFunction<glUniform3fv>();
            _glUniform4fv = LoadFunction<glUniform4fv>();
            _glUniform1iv = LoadFunction<glUniform1iv>();
            _glUniform2iv = LoadFunction<glUniform2iv>();
            _glUniform3iv = LoadFunction<glUniform3iv>();
            _glUniform4iv = LoadFunction<glUniform4iv>();
            _glUniformMatrix2fv = LoadFunction<glUniformMatrix2fv>();
            _glUniformMatrix3fv = LoadFunction<glUniformMatrix3fv>();
            _glUniformMatrix4fv = LoadFunction<glUniformMatrix4fv>();
            _glValidateProgram = LoadFunction<glValidateProgram>();
            _glVertexAttrib1d = LoadFunction<glVertexAttrib1d>();
            _glVertexAttrib1dv = LoadFunction<glVertexAttrib1dv>();
            _glVertexAttrib1f = LoadFunction<glVertexAttrib1f>();
            _glVertexAttrib1fv = LoadFunction<glVertexAttrib1fv>();
            _glVertexAttrib1s = LoadFunction<glVertexAttrib1s>();
            _glVertexAttrib1sv = LoadFunction<glVertexAttrib1sv>();
            _glVertexAttrib2d = LoadFunction<glVertexAttrib2d>();
            _glVertexAttrib2dv = LoadFunction<glVertexAttrib2dv>();
            _glVertexAttrib2f = LoadFunction<glVertexAttrib2f>();
            _glVertexAttrib2fv = LoadFunction<glVertexAttrib2fv>();
            _glVertexAttrib2s = LoadFunction<glVertexAttrib2s>();
            _glVertexAttrib2sv = LoadFunction<glVertexAttrib2sv>();
            _glVertexAttrib3d = LoadFunction<glVertexAttrib3d>();
            _glVertexAttrib3dv = LoadFunction<glVertexAttrib3dv>();
            _glVertexAttrib3f = LoadFunction<glVertexAttrib3f>();
            _glVertexAttrib3fv = LoadFunction<glVertexAttrib3fv>();
            _glVertexAttrib3s = LoadFunction<glVertexAttrib3s>();
            _glVertexAttrib3sv = LoadFunction<glVertexAttrib3sv>();
            _glVertexAttrib4Nbv = LoadFunction<glVertexAttrib4Nbv>();
            _glVertexAttrib4Niv = LoadFunction<glVertexAttrib4Niv>();
            _glVertexAttrib4Nsv = LoadFunction<glVertexAttrib4Nsv>();
            _glVertexAttrib4Nub = LoadFunction<glVertexAttrib4Nub>();
            _glVertexAttrib4Nubv = LoadFunction<glVertexAttrib4Nubv>();
            _glVertexAttrib4Nuiv = LoadFunction<glVertexAttrib4Nuiv>();
            _glVertexAttrib4Nusv = LoadFunction<glVertexAttrib4Nusv>();
            _glVertexAttrib4bv = LoadFunction<glVertexAttrib4bv>();
            _glVertexAttrib4d = LoadFunction<glVertexAttrib4d>();
            _glVertexAttrib4dv = LoadFunction<glVertexAttrib4dv>();
            _glVertexAttrib4f = LoadFunction<glVertexAttrib4f>();
            _glVertexAttrib4fv = LoadFunction<glVertexAttrib4fv>();
            _glVertexAttrib4iv = LoadFunction<glVertexAttrib4iv>();
            _glVertexAttrib4s = LoadFunction<glVertexAttrib4s>();
            _glVertexAttrib4sv = LoadFunction<glVertexAttrib4sv>();
            _glVertexAttrib4ubv = LoadFunction<glVertexAttrib4ubv>();
            _glVertexAttrib4uiv = LoadFunction<glVertexAttrib4uiv>();
            _glVertexAttrib4usv = LoadFunction<glVertexAttrib4usv>();
            _glVertexAttribPointer = LoadFunction<glVertexAttribPointer>();
            _glUniformMatrix2x3fv = LoadFunction<glUniformMatrix2x3fv>();
            _glUniformMatrix3x2fv = LoadFunction<glUniformMatrix3x2fv>();
            _glUniformMatrix2x4fv = LoadFunction<glUniformMatrix2x4fv>();
            _glUniformMatrix4x2fv = LoadFunction<glUniformMatrix4x2fv>();
            _glUniformMatrix3x4fv = LoadFunction<glUniformMatrix3x4fv>();
            _glUniformMatrix4x3fv = LoadFunction<glUniformMatrix4x3fv>();
            _glColorMaski = LoadFunction<glColorMaski>();
            _glGetBooleani_v = LoadFunction<glGetBooleani_v>();
            _glGetIntegeri_v = LoadFunction<glGetIntegeri_v>();
            _glEnablei = LoadFunction<glEnablei>();
            _glDisablei = LoadFunction<glDisablei>();
            _glIsEnabledi = LoadFunction<glIsEnabledi>();
            _glBeginTransformFeedback = LoadFunction<glBeginTransformFeedback>();
            _glEndTransformFeedback = LoadFunction<glEndTransformFeedback>();
            _glBindBufferRange = LoadFunction<glBindBufferRange>();
            _glBindBufferBase = LoadFunction<glBindBufferBase>();
            _glTransformFeedbackVaryings = LoadFunction<glTransformFeedbackVaryings>();
            _glGetTransformFeedbackVarying = LoadFunction<glGetTransformFeedbackVarying>();
            _glClampColor = LoadFunction<glClampColor>();
            _glBeginConditionalRender = LoadFunction<glBeginConditionalRender>();
            _glEndConditionalRender = LoadFunction<glEndConditionalRender>();
            _glVertexAttribIPointer = LoadFunction<glVertexAttribIPointer>();
            _glGetVertexAttribIiv = LoadFunction<glGetVertexAttribIiv>();
            _glGetVertexAttribIuiv = LoadFunction<glGetVertexAttribIuiv>();
            _glVertexAttribI1i = LoadFunction<glVertexAttribI1i>();
            _glVertexAttribI2i = LoadFunction<glVertexAttribI2i>();
            _glVertexAttribI3i = LoadFunction<glVertexAttribI3i>();
            _glVertexAttribI4i = LoadFunction<glVertexAttribI4i>();
            _glVertexAttribI1ui = LoadFunction<glVertexAttribI1ui>();
            _glVertexAttribI2ui = LoadFunction<glVertexAttribI2ui>();
            _glVertexAttribI3ui = LoadFunction<glVertexAttribI3ui>();
            _glVertexAttribI4ui = LoadFunction<glVertexAttribI4ui>();
            _glVertexAttribI1iv = LoadFunction<glVertexAttribI1iv>();
            _glVertexAttribI2iv = LoadFunction<glVertexAttribI2iv>();
            _glVertexAttribI3iv = LoadFunction<glVertexAttribI3iv>();
            _glVertexAttribI4iv = LoadFunction<glVertexAttribI4iv>();
            _glVertexAttribI1uiv = LoadFunction<glVertexAttribI1uiv>();
            _glVertexAttribI2uiv = LoadFunction<glVertexAttribI2uiv>();
            _glVertexAttribI3uiv = LoadFunction<glVertexAttribI3uiv>();
            _glVertexAttribI4uiv = LoadFunction<glVertexAttribI4uiv>();
            _glVertexAttribI4bv = LoadFunction<glVertexAttribI4bv>();
            _glVertexAttribI4sv = LoadFunction<glVertexAttribI4sv>();
            _glVertexAttribI4ubv = LoadFunction<glVertexAttribI4ubv>();
            _glVertexAttribI4usv = LoadFunction<glVertexAttribI4usv>();
            _glGetUniformuiv = LoadFunction<glGetUniformuiv>();
            _glBindFragDataLocation = LoadFunction<glBindFragDataLocation>();
            _glGetFragDataLocation = LoadFunction<glGetFragDataLocation>();
            _glUniform1ui = LoadFunction<glUniform1ui>();
            _glUniform2ui = LoadFunction<glUniform2ui>();
            _glUniform3ui = LoadFunction<glUniform3ui>();
            _glUniform4ui = LoadFunction<glUniform4ui>();
            _glUniform1uiv = LoadFunction<glUniform1uiv>();
            _glUniform2uiv = LoadFunction<glUniform2uiv>();
            _glUniform3uiv = LoadFunction<glUniform3uiv>();
            _glUniform4uiv = LoadFunction<glUniform4uiv>();
            _glTexParameterIiv = LoadFunction<glTexParameterIiv>();
            _glTexParameterIuiv = LoadFunction<glTexParameterIuiv>();
            _glGetTexParameterIiv = LoadFunction<glGetTexParameterIiv>();
            _glGetTexParameterIuiv = LoadFunction<glGetTexParameterIuiv>();
            _glClearBufferiv = LoadFunction<glClearBufferiv>();
            _glClearBufferuiv = LoadFunction<glClearBufferuiv>();
            _glClearBufferfv = LoadFunction<glClearBufferfv>();
            _glClearBufferfi = LoadFunction<glClearBufferfi>();
            _glGetStringi = LoadFunction<glGetStringi>();
            _glIsRenderbuffer = LoadFunction<glIsRenderbuffer>();
            _glBindRenderbuffer = LoadFunction<glBindRenderbuffer>();
            _glDeleteRenderbuffers = LoadFunction<glDeleteRenderbuffers>();
            _glGenRenderbuffers = LoadFunction<glGenRenderbuffers>();
            _glRenderbufferStorage = LoadFunction<glRenderbufferStorage>();
            _glGetRenderbufferParameteriv = LoadFunction<glGetRenderbufferParameteriv>();
            _glIsFramebuffer = LoadFunction<glIsFramebuffer>();
            _glBindFramebuffer = LoadFunction<glBindFramebuffer>();
            _glDeleteFramebuffers = LoadFunction<glDeleteFramebuffers>();
            _glGenFramebuffers = LoadFunction<glGenFramebuffers>();
            _glCheckFramebufferStatus = LoadFunction<glCheckFramebufferStatus>();
            _glFramebufferTexture1D = LoadFunction<glFramebufferTexture1D>();
            _glFramebufferTexture2D = LoadFunction<glFramebufferTexture2D>();
            _glFramebufferTexture3D = LoadFunction<glFramebufferTexture3D>();
            _glFramebufferRenderbuffer = LoadFunction<glFramebufferRenderbuffer>();
            _glGetFramebufferAttachmentParameteriv = LoadFunction<glGetFramebufferAttachmentParameteriv>();
            _glGenerateMipmap = LoadFunction<glGenerateMipmap>();
            _glBlitFramebuffer = LoadFunction<glBlitFramebuffer>();
            _glRenderbufferStorageMultisample = LoadFunction<glRenderbufferStorageMultisample>();
            _glFramebufferTextureLayer = LoadFunction<glFramebufferTextureLayer>();
            _glMapBufferRange = LoadFunction<glMapBufferRange>();
            _glFlushMappedBufferRange = LoadFunction<glFlushMappedBufferRange>();
            _glBindVertexArray = LoadFunction<glBindVertexArray>();
            _glDeleteVertexArrays = LoadFunction<glDeleteVertexArrays>();
            _glGenVertexArrays = LoadFunction<glGenVertexArrays>();
            _glIsVertexArray = LoadFunction<glIsVertexArray>();
            _glDrawArraysInstanced = LoadFunction<glDrawArraysInstanced>();
            _glDrawElementsInstanced = LoadFunction<glDrawElementsInstanced>();
            _glTexBuffer = LoadFunction<glTexBuffer>();
            _glPrimitiveRestartIndex = LoadFunction<glPrimitiveRestartIndex>();
            _glCopyBufferSubData = LoadFunction<glCopyBufferSubData>();
            _glGetUniformIndices = LoadFunction<glGetUniformIndices>();
            _glGetActiveUniformsiv = LoadFunction<glGetActiveUniformsiv>();
            _glGetActiveUniformName = LoadFunction<glGetActiveUniformName>();
            _glGetUniformBlockIndex = LoadFunction<glGetUniformBlockIndex>();
            _glGetActiveUniformBlockiv = LoadFunction<glGetActiveUniformBlockiv>();
            _glGetActiveUniformBlockName = LoadFunction<glGetActiveUniformBlockName>();
            _glUniformBlockBinding = LoadFunction<glUniformBlockBinding>();
            _glDrawElementsBaseVertex = LoadFunction<glDrawElementsBaseVertex>();
            _glDrawRangeElementsBaseVertex = LoadFunction<glDrawRangeElementsBaseVertex>();
            _glDrawElementsInstancedBaseVertex = LoadFunction<glDrawElementsInstancedBaseVertex>();
            _glMultiDrawElementsBaseVertex = LoadFunction<glMultiDrawElementsBaseVertex>();
            _glProvokingVertex = LoadFunction<glProvokingVertex>();
            _glFenceSync = LoadFunction<glFenceSync>();
            _glIsSync = LoadFunction<glIsSync>();
            _glDeleteSync = LoadFunction<glDeleteSync>();
            _glClientWaitSync = LoadFunction<glClientWaitSync>();
            _glWaitSync = LoadFunction<glWaitSync>();
            _glGetInteger64v = LoadFunction<glGetInteger64v>();
            _glGetSynciv = LoadFunction<glGetSynciv>();
            _glGetInteger64i_v = LoadFunction<glGetInteger64i_v>();
            _glGetBufferParameteri64v = LoadFunction<glGetBufferParameteri64v>();
            _glFramebufferTexture = LoadFunction<glFramebufferTexture>();
            _glTexImage2DMultisample = LoadFunction<glTexImage2DMultisample>();
            _glTexImage3DMultisample = LoadFunction<glTexImage3DMultisample>();
            _glGetMultisamplefv = LoadFunction<glGetMultisamplefv>();
            _glSampleMaski = LoadFunction<glSampleMaski>();
            _glBindFragDataLocationIndexed = LoadFunction<glBindFragDataLocationIndexed>();
            _glGetFragDataIndex = LoadFunction<glGetFragDataIndex>();
            _glGenSamplers = LoadFunction<glGenSamplers>();
            _glDeleteSamplers = LoadFunction<glDeleteSamplers>();
            _glIsSampler = LoadFunction<glIsSampler>();
            _glBindSampler = LoadFunction<glBindSampler>();
            _glSamplerParameteri = LoadFunction<glSamplerParameteri>();
            _glSamplerParameteriv = LoadFunction<glSamplerParameteriv>();
            _glSamplerParameterf = LoadFunction<glSamplerParameterf>();
            _glSamplerParameterfv = LoadFunction<glSamplerParameterfv>();
            _glSamplerParameterIiv = LoadFunction<glSamplerParameterIiv>();
            _glSamplerParameterIuiv = LoadFunction<glSamplerParameterIuiv>();
            _glGetSamplerParameteriv = LoadFunction<glGetSamplerParameteriv>();
            _glGetSamplerParameterIiv = LoadFunction<glGetSamplerParameterIiv>();
            _glGetSamplerParameterfv = LoadFunction<glGetSamplerParameterfv>();
            _glGetSamplerParameterIuiv = LoadFunction<glGetSamplerParameterIuiv>();
            _glQueryCounter = LoadFunction<glQueryCounter>();
            _glGetQueryObjecti64v = LoadFunction<glGetQueryObjecti64v>();
            _glGetQueryObjectui64v = LoadFunction<glGetQueryObjectui64v>();
            _glVertexAttribDivisor = LoadFunction<glVertexAttribDivisor>();
            _glVertexAttribP1ui = LoadFunction<glVertexAttribP1ui>();
            _glVertexAttribP1uiv = LoadFunction<glVertexAttribP1uiv>();
            _glVertexAttribP2ui = LoadFunction<glVertexAttribP2ui>();
            _glVertexAttribP2uiv = LoadFunction<glVertexAttribP2uiv>();
            _glVertexAttribP3ui = LoadFunction<glVertexAttribP3ui>();
            _glVertexAttribP3uiv = LoadFunction<glVertexAttribP3uiv>();
            _glVertexAttribP4ui = LoadFunction<glVertexAttribP4ui>();
            _glVertexAttribP4uiv = LoadFunction<glVertexAttribP4uiv>();
            _glMinSampleShading = LoadFunction<glMinSampleShading>();
            _glBlendEquationi = LoadFunction<glBlendEquationi>();
            _glBlendEquationSeparatei = LoadFunction<glBlendEquationSeparatei>();
            _glBlendFunci = LoadFunction<glBlendFunci>();
            _glBlendFuncSeparatei = LoadFunction<glBlendFuncSeparatei>();
            _glDrawArraysIndirect = LoadFunction<glDrawArraysIndirect>();
            _glDrawElementsIndirect = LoadFunction<glDrawElementsIndirect>();
            _glUniform1d = LoadFunction<glUniform1d>();
            _glUniform2d = LoadFunction<glUniform2d>();
            _glUniform3d = LoadFunction<glUniform3d>();
            _glUniform4d = LoadFunction<glUniform4d>();
            _glUniform1dv = LoadFunction<glUniform1dv>();
            _glUniform2dv = LoadFunction<glUniform2dv>();
            _glUniform3dv = LoadFunction<glUniform3dv>();
            _glUniform4dv = LoadFunction<glUniform4dv>();
            _glUniformMatrix2dv = LoadFunction<glUniformMatrix2dv>();
            _glUniformMatrix3dv = LoadFunction<glUniformMatrix3dv>();
            _glUniformMatrix4dv = LoadFunction<glUniformMatrix4dv>();
            _glUniformMatrix2x3dv = LoadFunction<glUniformMatrix2x3dv>();
            _glUniformMatrix2x4dv = LoadFunction<glUniformMatrix2x4dv>();
            _glUniformMatrix3x2dv = LoadFunction<glUniformMatrix3x2dv>();
            _glUniformMatrix3x4dv = LoadFunction<glUniformMatrix3x4dv>();
            _glUniformMatrix4x2dv = LoadFunction<glUniformMatrix4x2dv>();
            _glUniformMatrix4x3dv = LoadFunction<glUniformMatrix4x3dv>();
            _glGetUniformdv = LoadFunction<glGetUniformdv>();
            _glGetSubroutineUniformLocation = LoadFunction<glGetSubroutineUniformLocation>();
            _glGetSubroutineIndex = LoadFunction<glGetSubroutineIndex>();
            _glGetActiveSubroutineUniformiv = LoadFunction<glGetActiveSubroutineUniformiv>();
            _glGetActiveSubroutineUniformName = LoadFunction<glGetActiveSubroutineUniformName>();
            _glGetActiveSubroutineName = LoadFunction<glGetActiveSubroutineName>();
            _glUniformSubroutinesuiv = LoadFunction<glUniformSubroutinesuiv>();
            _glGetUniformSubroutineuiv = LoadFunction<glGetUniformSubroutineuiv>();
            _glGetProgramStageiv = LoadFunction<glGetProgramStageiv>();
            _glPatchParameteri = LoadFunction<glPatchParameteri>();
            _glPatchParameterfv = LoadFunction<glPatchParameterfv>();
            _glBindTransformFeedback = LoadFunction<glBindTransformFeedback>();
            _glDeleteTransformFeedbacks = LoadFunction<glDeleteTransformFeedbacks>();
            _glGenTransformFeedbacks = LoadFunction<glGenTransformFeedbacks>();
            _glIsTransformFeedback = LoadFunction<glIsTransformFeedback>();
            _glPauseTransformFeedback = LoadFunction<glPauseTransformFeedback>();
            _glResumeTransformFeedback = LoadFunction<glResumeTransformFeedback>();
            _glDrawTransformFeedback = LoadFunction<glDrawTransformFeedback>();
            _glDrawTransformFeedbackStream = LoadFunction<glDrawTransformFeedbackStream>();
            _glBeginQueryIndexed = LoadFunction<glBeginQueryIndexed>();
            _glEndQueryIndexed = LoadFunction<glEndQueryIndexed>();
            _glGetQueryIndexediv = LoadFunction<glGetQueryIndexediv>();
            _glReleaseShaderCompiler = LoadFunction<glReleaseShaderCompiler>();
            _glShaderBinary = LoadFunction<glShaderBinary>();
            _glGetShaderPrecisionFormat = LoadFunction<glGetShaderPrecisionFormat>();
            _glDepthRangef = LoadFunction<glDepthRangef>();
            _glClearDepthf = LoadFunction<glClearDepthf>();
            _glGetProgramBinary = LoadFunction<glGetProgramBinary>();
            _glProgramBinary = LoadFunction<glProgramBinary>();
            _glProgramParameteri = LoadFunction<glProgramParameteri>();
            _glUseProgramStages = LoadFunction<glUseProgramStages>();
            _glActiveShaderProgram = LoadFunction<glActiveShaderProgram>();
            _glCreateShaderProgramv = LoadFunction<glCreateShaderProgramv>();
            _glBindProgramPipeline = LoadFunction<glBindProgramPipeline>();
            _glDeleteProgramPipelines = LoadFunction<glDeleteProgramPipelines>();
            _glGenProgramPipelines = LoadFunction<glGenProgramPipelines>();
            _glIsProgramPipeline = LoadFunction<glIsProgramPipeline>();
            _glGetProgramPipelineiv = LoadFunction<glGetProgramPipelineiv>();
            _glProgramUniform1i = LoadFunction<glProgramUniform1i>();
            _glProgramUniform1iv = LoadFunction<glProgramUniform1iv>();
            _glProgramUniform1f = LoadFunction<glProgramUniform1f>();
            _glProgramUniform1fv = LoadFunction<glProgramUniform1fv>();
            _glProgramUniform1d = LoadFunction<glProgramUniform1d>();
            _glProgramUniform1dv = LoadFunction<glProgramUniform1dv>();
            _glProgramUniform1ui = LoadFunction<glProgramUniform1ui>();
            _glProgramUniform1uiv = LoadFunction<glProgramUniform1uiv>();
            _glProgramUniform2i = LoadFunction<glProgramUniform2i>();
            _glProgramUniform2iv = LoadFunction<glProgramUniform2iv>();
            _glProgramUniform2f = LoadFunction<glProgramUniform2f>();
            _glProgramUniform2fv = LoadFunction<glProgramUniform2fv>();
            _glProgramUniform2d = LoadFunction<glProgramUniform2d>();
            _glProgramUniform2dv = LoadFunction<glProgramUniform2dv>();
            _glProgramUniform2ui = LoadFunction<glProgramUniform2ui>();
            _glProgramUniform2uiv = LoadFunction<glProgramUniform2uiv>();
            _glProgramUniform3i = LoadFunction<glProgramUniform3i>();
            _glProgramUniform3iv = LoadFunction<glProgramUniform3iv>();
            _glProgramUniform3f = LoadFunction<glProgramUniform3f>();
            _glProgramUniform3fv = LoadFunction<glProgramUniform3fv>();
            _glProgramUniform3d = LoadFunction<glProgramUniform3d>();
            _glProgramUniform3dv = LoadFunction<glProgramUniform3dv>();
            _glProgramUniform3ui = LoadFunction<glProgramUniform3ui>();
            _glProgramUniform3uiv = LoadFunction<glProgramUniform3uiv>();
            _glProgramUniform4i = LoadFunction<glProgramUniform4i>();
            _glProgramUniform4iv = LoadFunction<glProgramUniform4iv>();
            _glProgramUniform4f = LoadFunction<glProgramUniform4f>();
            _glProgramUniform4fv = LoadFunction<glProgramUniform4fv>();
            _glProgramUniform4d = LoadFunction<glProgramUniform4d>();
            _glProgramUniform4dv = LoadFunction<glProgramUniform4dv>();
            _glProgramUniform4ui = LoadFunction<glProgramUniform4ui>();
            _glProgramUniform4uiv = LoadFunction<glProgramUniform4uiv>();
            _glProgramUniformMatrix2fv = LoadFunction<glProgramUniformMatrix2fv>();
            _glProgramUniformMatrix3fv = LoadFunction<glProgramUniformMatrix3fv>();
            _glProgramUniformMatrix4fv = LoadFunction<glProgramUniformMatrix4fv>();
            _glProgramUniformMatrix2dv = LoadFunction<glProgramUniformMatrix2dv>();
            _glProgramUniformMatrix3dv = LoadFunction<glProgramUniformMatrix3dv>();
            _glProgramUniformMatrix4dv = LoadFunction<glProgramUniformMatrix4dv>();
            _glProgramUniformMatrix2x3fv = LoadFunction<glProgramUniformMatrix2x3fv>();
            _glProgramUniformMatrix3x2fv = LoadFunction<glProgramUniformMatrix3x2fv>();
            _glProgramUniformMatrix2x4fv = LoadFunction<glProgramUniformMatrix2x4fv>();
            _glProgramUniformMatrix4x2fv = LoadFunction<glProgramUniformMatrix4x2fv>();
            _glProgramUniformMatrix3x4fv = LoadFunction<glProgramUniformMatrix3x4fv>();
            _glProgramUniformMatrix4x3fv = LoadFunction<glProgramUniformMatrix4x3fv>();
            _glProgramUniformMatrix2x3dv = LoadFunction<glProgramUniformMatrix2x3dv>();
            _glProgramUniformMatrix3x2dv = LoadFunction<glProgramUniformMatrix3x2dv>();
            _glProgramUniformMatrix2x4dv = LoadFunction<glProgramUniformMatrix2x4dv>();
            _glProgramUniformMatrix4x2dv = LoadFunction<glProgramUniformMatrix4x2dv>();
            _glProgramUniformMatrix3x4dv = LoadFunction<glProgramUniformMatrix3x4dv>();
            _glProgramUniformMatrix4x3dv = LoadFunction<glProgramUniformMatrix4x3dv>();
            _glValidateProgramPipeline = LoadFunction<glValidateProgramPipeline>();
            _glGetProgramPipelineInfoLog = LoadFunction<glGetProgramPipelineInfoLog>();
            _glVertexAttribL1d = LoadFunction<glVertexAttribL1d>();
            _glVertexAttribL2d = LoadFunction<glVertexAttribL2d>();
            _glVertexAttribL3d = LoadFunction<glVertexAttribL3d>();
            _glVertexAttribL4d = LoadFunction<glVertexAttribL4d>();
            _glVertexAttribL1dv = LoadFunction<glVertexAttribL1dv>();
            _glVertexAttribL2dv = LoadFunction<glVertexAttribL2dv>();
            _glVertexAttribL3dv = LoadFunction<glVertexAttribL3dv>();
            _glVertexAttribL4dv = LoadFunction<glVertexAttribL4dv>();
            _glVertexAttribLPointer = LoadFunction<glVertexAttribLPointer>();
            _glGetVertexAttribLdv = LoadFunction<glGetVertexAttribLdv>();
            _glViewportArrayv = LoadFunction<glViewportArrayv>();
            _glViewportIndexedf = LoadFunction<glViewportIndexedf>();
            _glViewportIndexedfv = LoadFunction<glViewportIndexedfv>();
            _glScissorArrayv = LoadFunction<glScissorArrayv>();
            _glScissorIndexed = LoadFunction<glScissorIndexed>();
            _glScissorIndexedv = LoadFunction<glScissorIndexedv>();
            _glDepthRangeArrayv = LoadFunction<glDepthRangeArrayv>();
            _glDepthRangeIndexed = LoadFunction<glDepthRangeIndexed>();
            _glGetFloati_v = LoadFunction<glGetFloati_v>();
            _glGetDoublei_v = LoadFunction<glGetDoublei_v>();
            _glDrawArraysInstancedBaseInstance = LoadFunction<glDrawArraysInstancedBaseInstance>();
            _glDrawElementsInstancedBaseInstance = LoadFunction<glDrawElementsInstancedBaseInstance>();
            _glDrawElementsInstancedBaseVertexBaseInstance = LoadFunction<glDrawElementsInstancedBaseVertexBaseInstance>();
            _glGetInternalformativ = LoadFunction<glGetInternalformativ>();
            _glGetActiveAtomicCounterBufferiv = LoadFunction<glGetActiveAtomicCounterBufferiv>();
            _glBindImageTexture = LoadFunction<glBindImageTexture>();
            _glMemoryBarrier = LoadFunction<glMemoryBarrier>();
            _glTexStorage1D = LoadFunction<glTexStorage1D>();
            _glTexStorage2D = LoadFunction<glTexStorage2D>();
            _glTexStorage3D = LoadFunction<glTexStorage3D>();
            _glDrawTransformFeedbackInstanced = LoadFunction<glDrawTransformFeedbackInstanced>();
            _glDrawTransformFeedbackStreamInstanced = LoadFunction<glDrawTransformFeedbackStreamInstanced>();
            _glClearBufferData = LoadFunction<glClearBufferData>();
            _glClearBufferSubData = LoadFunction<glClearBufferSubData>();
            _glDispatchCompute = LoadFunction<glDispatchCompute>();
            _glDispatchComputeIndirect = LoadFunction<glDispatchComputeIndirect>();
            _glCopyImageSubData = LoadFunction<glCopyImageSubData>();
            _glFramebufferParameteri = LoadFunction<glFramebufferParameteri>();
            _glGetFramebufferParameteriv = LoadFunction<glGetFramebufferParameteriv>();
            _glGetInternalformati64v = LoadFunction<glGetInternalformati64v>();
            _glInvalidateTexSubImage = LoadFunction<glInvalidateTexSubImage>();
            _glInvalidateTexImage = LoadFunction<glInvalidateTexImage>();
            _glInvalidateBufferSubData = LoadFunction<glInvalidateBufferSubData>();
            _glInvalidateBufferData = LoadFunction<glInvalidateBufferData>();
            _glInvalidateFramebuffer = LoadFunction<glInvalidateFramebuffer>();
            _glInvalidateSubFramebuffer = LoadFunction<glInvalidateSubFramebuffer>();
            _glMultiDrawArraysIndirect = LoadFunction<glMultiDrawArraysIndirect>();
            _glMultiDrawElementsIndirect = LoadFunction<glMultiDrawElementsIndirect>();
            _glGetProgramInterfaceiv = LoadFunction<glGetProgramInterfaceiv>();
            _glGetProgramResourceIndex = LoadFunction<glGetProgramResourceIndex>();
            _glGetProgramResourceName = LoadFunction<glGetProgramResourceName>();
            _glGetProgramResourceiv = LoadFunction<glGetProgramResourceiv>();
            _glGetProgramResourceLocation = LoadFunction<glGetProgramResourceLocation>();
            _glGetProgramResourceLocationIndex = LoadFunction<glGetProgramResourceLocationIndex>();
            _glShaderStorageBlockBinding = LoadFunction<glShaderStorageBlockBinding>();
            _glTexBufferRange = LoadFunction<glTexBufferRange>();
            _glTexStorage2DMultisample = LoadFunction<glTexStorage2DMultisample>();
            _glTexStorage3DMultisample = LoadFunction<glTexStorage3DMultisample>();
            _glTextureView = LoadFunction<glTextureView>();
            _glBindVertexBuffer = LoadFunction<glBindVertexBuffer>();
            _glVertexAttribFormat = LoadFunction<glVertexAttribFormat>();
            _glVertexAttribIFormat = LoadFunction<glVertexAttribIFormat>();
            _glVertexAttribLFormat = LoadFunction<glVertexAttribLFormat>();
            _glVertexAttribBinding = LoadFunction<glVertexAttribBinding>();
            _glVertexBindingDivisor = LoadFunction<glVertexBindingDivisor>();
            _glDebugMessageControl = LoadFunction<glDebugMessageControl>();
            _glDebugMessageInsert = LoadFunction<glDebugMessageInsert>();
            _glDebugMessageCallback = LoadFunction<glDebugMessageCallback>();
            _glGetDebugMessageLog = LoadFunction<glGetDebugMessageLog>();
            _glPushDebugGroup = LoadFunction<glPushDebugGroup>();
            _glPopDebugGroup = LoadFunction<glPopDebugGroup>();
            _glObjectLabel = LoadFunction<glObjectLabel>();
            _glGetObjectLabel = LoadFunction<glGetObjectLabel>();
            _glObjectPtrLabel = LoadFunction<glObjectPtrLabel>();
            _glGetObjectPtrLabel = LoadFunction<glGetObjectPtrLabel>();
            _glGetPointerv = LoadFunction<glGetPointerv>();
            _glBufferStorage = LoadFunction<glBufferStorage>();
            _glClearTexImage = LoadFunction<glClearTexImage>();
            _glClearTexSubImage = LoadFunction<glClearTexSubImage>();
            _glBindBuffersBase = LoadFunction<glBindBuffersBase>();
            _glBindBuffersRange = LoadFunction<glBindBuffersRange>();
            _glBindTextures = LoadFunction<glBindTextures>();
            _glBindSamplers = LoadFunction<glBindSamplers>();
            _glBindImageTextures = LoadFunction<glBindImageTextures>();
            _glBindVertexBuffers = LoadFunction<glBindVertexBuffers>();
            _glClipControl = LoadFunction<glClipControl>();
            _glCreateTransformFeedbacks = LoadFunction<glCreateTransformFeedbacks>();
            _glTransformFeedbackBufferBase = LoadFunction<glTransformFeedbackBufferBase>();
            _glTransformFeedbackBufferRange = LoadFunction<glTransformFeedbackBufferRange>();
            _glGetTransformFeedbackiv = LoadFunction<glGetTransformFeedbackiv>();
            _glGetTransformFeedbacki_v = LoadFunction<glGetTransformFeedbacki_v>();
            _glGetTransformFeedbacki64_v = LoadFunction<glGetTransformFeedbacki64_v>();
            _glCreateBuffers = LoadFunction<glCreateBuffers>();
            _glNamedBufferStorage = LoadFunction<glNamedBufferStorage>();
            _glNamedBufferData = LoadFunction<glNamedBufferData>();
            _glNamedBufferSubData = LoadFunction<glNamedBufferSubData>();
            _glCopyNamedBufferSubData = LoadFunction<glCopyNamedBufferSubData>();
            _glClearNamedBufferData = LoadFunction<glClearNamedBufferData>();
            _glClearNamedBufferSubData = LoadFunction<glClearNamedBufferSubData>();
            _glMapNamedBuffer = LoadFunction<glMapNamedBuffer>();
            _glMapNamedBufferRange = LoadFunction<glMapNamedBufferRange>();
            _glUnmapNamedBuffer = LoadFunction<glUnmapNamedBuffer>();
            _glFlushMappedNamedBufferRange = LoadFunction<glFlushMappedNamedBufferRange>();
            _glGetNamedBufferParameteriv = LoadFunction<glGetNamedBufferParameteriv>();
            _glGetNamedBufferParameteri64v = LoadFunction<glGetNamedBufferParameteri64v>();
            _glGetNamedBufferPointerv = LoadFunction<glGetNamedBufferPointerv>();
            _glGetNamedBufferSubData = LoadFunction<glGetNamedBufferSubData>();
            _glCreateFramebuffers = LoadFunction<glCreateFramebuffers>();
            _glNamedFramebufferRenderbuffer = LoadFunction<glNamedFramebufferRenderbuffer>();
            _glNamedFramebufferParameteri = LoadFunction<glNamedFramebufferParameteri>();
            _glNamedFramebufferTexture = LoadFunction<glNamedFramebufferTexture>();
            _glNamedFramebufferTextureLayer = LoadFunction<glNamedFramebufferTextureLayer>();
            _glNamedFramebufferDrawBuffer = LoadFunction<glNamedFramebufferDrawBuffer>();
            _glNamedFramebufferDrawBuffers = LoadFunction<glNamedFramebufferDrawBuffers>();
            _glNamedFramebufferReadBuffer = LoadFunction<glNamedFramebufferReadBuffer>();
            _glInvalidateNamedFramebufferData = LoadFunction<glInvalidateNamedFramebufferData>();
            _glInvalidateNamedFramebufferSubData = LoadFunction<glInvalidateNamedFramebufferSubData>();
            _glClearNamedFramebufferiv = LoadFunction<glClearNamedFramebufferiv>();
            _glClearNamedFramebufferuiv = LoadFunction<glClearNamedFramebufferuiv>();
            _glClearNamedFramebufferfv = LoadFunction<glClearNamedFramebufferfv>();
            _glClearNamedFramebufferfi = LoadFunction<glClearNamedFramebufferfi>();
            _glBlitNamedFramebuffer = LoadFunction<glBlitNamedFramebuffer>();
            _glCheckNamedFramebufferStatus = LoadFunction<glCheckNamedFramebufferStatus>();
            _glGetNamedFramebufferParameteriv = LoadFunction<glGetNamedFramebufferParameteriv>();
            _glGetNamedFramebufferAttachmentParameteriv = LoadFunction<glGetNamedFramebufferAttachmentParameteriv>();
            _glCreateRenderbuffers = LoadFunction<glCreateRenderbuffers>();
            _glNamedRenderbufferStorage = LoadFunction<glNamedRenderbufferStorage>();
            _glNamedRenderbufferStorageMultisample = LoadFunction<glNamedRenderbufferStorageMultisample>();
            _glGetNamedRenderbufferParameteriv = LoadFunction<glGetNamedRenderbufferParameteriv>();
            _glCreateTextures = LoadFunction<glCreateTextures>();
            _glTextureBuffer = LoadFunction<glTextureBuffer>();
            _glTextureBufferRange = LoadFunction<glTextureBufferRange>();
            _glTextureStorage1D = LoadFunction<glTextureStorage1D>();
            _glTextureStorage2D = LoadFunction<glTextureStorage2D>();
            _glTextureStorage3D = LoadFunction<glTextureStorage3D>();
            _glTextureStorage2DMultisample = LoadFunction<glTextureStorage2DMultisample>();
            _glTextureStorage3DMultisample = LoadFunction<glTextureStorage3DMultisample>();
            _glTextureSubImage1D = LoadFunction<glTextureSubImage1D>();
            _glTextureSubImage2D = LoadFunction<glTextureSubImage2D>();
            _glTextureSubImage3D = LoadFunction<glTextureSubImage3D>();
            _glCompressedTextureSubImage1D = LoadFunction<glCompressedTextureSubImage1D>();
            _glCompressedTextureSubImage2D = LoadFunction<glCompressedTextureSubImage2D>();
            _glCompressedTextureSubImage3D = LoadFunction<glCompressedTextureSubImage3D>();
            _glCopyTextureSubImage1D = LoadFunction<glCopyTextureSubImage1D>();
            _glCopyTextureSubImage2D = LoadFunction<glCopyTextureSubImage2D>();
            _glCopyTextureSubImage3D = LoadFunction<glCopyTextureSubImage3D>();
            _glTextureParameterf = LoadFunction<glTextureParameterf>();
            _glTextureParameterfv = LoadFunction<glTextureParameterfv>();
            _glTextureParameteri = LoadFunction<glTextureParameteri>();
            _glTextureParameterIiv = LoadFunction<glTextureParameterIiv>();
            _glTextureParameterIuiv = LoadFunction<glTextureParameterIuiv>();
            _glTextureParameteriv = LoadFunction<glTextureParameteriv>();
            _glGenerateTextureMipmap = LoadFunction<glGenerateTextureMipmap>();
            _glBindTextureUnit = LoadFunction<glBindTextureUnit>();
            _glGetTextureImage = LoadFunction<glGetTextureImage>();
            _glGetCompressedTextureImage = LoadFunction<glGetCompressedTextureImage>();
            _glGetTextureLevelParameterfv = LoadFunction<glGetTextureLevelParameterfv>();
            _glGetTextureLevelParameteriv = LoadFunction<glGetTextureLevelParameteriv>();
            _glGetTextureParameterfv = LoadFunction<glGetTextureParameterfv>();
            _glGetTextureParameterIiv = LoadFunction<glGetTextureParameterIiv>();
            _glGetTextureParameterIuiv = LoadFunction<glGetTextureParameterIuiv>();
            _glGetTextureParameteriv = LoadFunction<glGetTextureParameteriv>();
            _glCreateVertexArrays = LoadFunction<glCreateVertexArrays>();
            _glDisableVertexArrayAttrib = LoadFunction<glDisableVertexArrayAttrib>();
            _glEnableVertexArrayAttrib = LoadFunction<glEnableVertexArrayAttrib>();
            _glVertexArrayElementBuffer = LoadFunction<glVertexArrayElementBuffer>();
            _glVertexArrayVertexBuffer = LoadFunction<glVertexArrayVertexBuffer>();
            _glVertexArrayVertexBuffers = LoadFunction<glVertexArrayVertexBuffers>();
            _glVertexArrayAttribBinding = LoadFunction<glVertexArrayAttribBinding>();
            _glVertexArrayAttribFormat = LoadFunction<glVertexArrayAttribFormat>();
            _glVertexArrayAttribIFormat = LoadFunction<glVertexArrayAttribIFormat>();
            _glVertexArrayAttribLFormat = LoadFunction<glVertexArrayAttribLFormat>();
            _glVertexArrayBindingDivisor = LoadFunction<glVertexArrayBindingDivisor>();
            _glGetVertexArrayiv = LoadFunction<glGetVertexArrayiv>();
            _glGetVertexArrayIndexediv = LoadFunction<glGetVertexArrayIndexediv>();
            _glGetVertexArrayIndexed64iv = LoadFunction<glGetVertexArrayIndexed64iv>();
            _glCreateSamplers = LoadFunction<glCreateSamplers>();
            _glCreateProgramPipelines = LoadFunction<glCreateProgramPipelines>();
            _glCreateQueries = LoadFunction<glCreateQueries>();
            _glGetQueryBufferObjecti64v = LoadFunction<glGetQueryBufferObjecti64v>();
            _glGetQueryBufferObjectiv = LoadFunction<glGetQueryBufferObjectiv>();
            _glGetQueryBufferObjectui64v = LoadFunction<glGetQueryBufferObjectui64v>();
            _glGetQueryBufferObjectuiv = LoadFunction<glGetQueryBufferObjectuiv>();
            _glMemoryBarrierByRegion = LoadFunction<glMemoryBarrierByRegion>();
            _glGetTextureSubImage = LoadFunction<glGetTextureSubImage>();
            _glGetCompressedTextureSubImage = LoadFunction<glGetCompressedTextureSubImage>();
            _glGetGraphicsResetStatus = LoadFunction<glGetGraphicsResetStatus>();
            _glGetnCompressedTexImage = LoadFunction<glGetnCompressedTexImage>();
            _glGetnTexImage = LoadFunction<glGetnTexImage>();
            _glGetnUniformdv = LoadFunction<glGetnUniformdv>();
            _glGetnUniformfv = LoadFunction<glGetnUniformfv>();
            _glGetnUniformiv = LoadFunction<glGetnUniformiv>();
            _glGetnUniformuiv = LoadFunction<glGetnUniformuiv>();
            _glReadnPixels = LoadFunction<glReadnPixels>();
            _glTextureBarrier = LoadFunction<glTextureBarrier>();
            _glGetPerfMonitorGroupsAMD = LoadFunction<glGetPerfMonitorGroupsAMD>();
            _glGetPerfMonitorCountersAMD = LoadFunction<glGetPerfMonitorCountersAMD>();
            _glGetPerfMonitorGroupStringAMD = LoadFunction<glGetPerfMonitorGroupStringAMD>();
            _glGetPerfMonitorCounterStringAMD = LoadFunction<glGetPerfMonitorCounterStringAMD>();
            _glGetPerfMonitorCounterInfoAMD = LoadFunction<glGetPerfMonitorCounterInfoAMD>();
            _glGenPerfMonitorsAMD = LoadFunction<glGenPerfMonitorsAMD>();
            _glDeletePerfMonitorsAMD = LoadFunction<glDeletePerfMonitorsAMD>();
            _glSelectPerfMonitorCountersAMD = LoadFunction<glSelectPerfMonitorCountersAMD>();
            _glBeginPerfMonitorAMD = LoadFunction<glBeginPerfMonitorAMD>();
            _glEndPerfMonitorAMD = LoadFunction<glEndPerfMonitorAMD>();
            _glGetPerfMonitorCounterDataAMD = LoadFunction<glGetPerfMonitorCounterDataAMD>();
            _glGetTextureHandleARB = LoadFunction<glGetTextureHandleARB>();
            _glGetTextureSamplerHandleARB = LoadFunction<glGetTextureSamplerHandleARB>();
            _glMakeTextureHandleResidentARB = LoadFunction<glMakeTextureHandleResidentARB>();
            _glMakeTextureHandleNonResidentARB = LoadFunction<glMakeTextureHandleNonResidentARB>();
            _glGetImageHandleARB = LoadFunction<glGetImageHandleARB>();
            _glMakeImageHandleResidentARB = LoadFunction<glMakeImageHandleResidentARB>();
            _glMakeImageHandleNonResidentARB = LoadFunction<glMakeImageHandleNonResidentARB>();
            _glUniformHandleui64ARB = LoadFunction<glUniformHandleui64ARB>();
            _glUniformHandleui64vARB = LoadFunction<glUniformHandleui64vARB>();
            _glProgramUniformHandleui64ARB = LoadFunction<glProgramUniformHandleui64ARB>();
            _glProgramUniformHandleui64vARB = LoadFunction<glProgramUniformHandleui64vARB>();
            _glIsTextureHandleResidentARB = LoadFunction<glIsTextureHandleResidentARB>();
            _glIsImageHandleResidentARB = LoadFunction<glIsImageHandleResidentARB>();
            _glVertexAttribL1ui64ARB = LoadFunction<glVertexAttribL1ui64ARB>();
            _glVertexAttribL1ui64vARB = LoadFunction<glVertexAttribL1ui64vARB>();
            _glGetVertexAttribLui64vARB = LoadFunction<glGetVertexAttribLui64vARB>();
            _glCreateSyncFromCLeventARB = LoadFunction<glCreateSyncFromCLeventARB>();
            _glDispatchComputeGroupSizeARB = LoadFunction<glDispatchComputeGroupSizeARB>();
            _glDebugMessageControlARB = LoadFunction<glDebugMessageControlARB>();
            _glDebugMessageInsertARB = LoadFunction<glDebugMessageInsertARB>();
            _glDebugMessageCallbackARB = LoadFunction<glDebugMessageCallbackARB>();
            _glGetDebugMessageLogARB = LoadFunction<glGetDebugMessageLogARB>();
            _glBlendEquationiARB = LoadFunction<glBlendEquationiARB>();
            _glBlendEquationSeparateiARB = LoadFunction<glBlendEquationSeparateiARB>();
            _glBlendFunciARB = LoadFunction<glBlendFunciARB>();
            _glBlendFuncSeparateiARB = LoadFunction<glBlendFuncSeparateiARB>();
            _glMultiDrawArraysIndirectCountARB = LoadFunction<glMultiDrawArraysIndirectCountARB>();
            _glMultiDrawElementsIndirectCountARB = LoadFunction<glMultiDrawElementsIndirectCountARB>();
            _glGetGraphicsResetStatusARB = LoadFunction<glGetGraphicsResetStatusARB>();
            _glGetnTexImageARB = LoadFunction<glGetnTexImageARB>();
            _glReadnPixelsARB = LoadFunction<glReadnPixelsARB>();
            _glGetnCompressedTexImageARB = LoadFunction<glGetnCompressedTexImageARB>();
            _glGetnUniformfvARB = LoadFunction<glGetnUniformfvARB>();
            _glGetnUniformivARB = LoadFunction<glGetnUniformivARB>();
            _glGetnUniformuivARB = LoadFunction<glGetnUniformuivARB>();
            _glGetnUniformdvARB = LoadFunction<glGetnUniformdvARB>();
            _glMinSampleShadingARB = LoadFunction<glMinSampleShadingARB>();
            _glNamedStringARB = LoadFunction<glNamedStringARB>();
            _glDeleteNamedStringARB = LoadFunction<glDeleteNamedStringARB>();
            _glCompileShaderIncludeARB = LoadFunction<glCompileShaderIncludeARB>();
            _glIsNamedStringARB = LoadFunction<glIsNamedStringARB>();
            _glGetNamedStringARB = LoadFunction<glGetNamedStringARB>();
            _glGetNamedStringivARB = LoadFunction<glGetNamedStringivARB>();
            _glBufferPageCommitmentARB = LoadFunction<glBufferPageCommitmentARB>();
            _glNamedBufferPageCommitmentEXT = LoadFunction<glNamedBufferPageCommitmentEXT>();
            _glNamedBufferPageCommitmentARB = LoadFunction<glNamedBufferPageCommitmentARB>();
            _glTexPageCommitmentARB = LoadFunction<glTexPageCommitmentARB>();
            _glLabelObjectEXT = LoadFunction<glLabelObjectEXT>();
            _glGetObjectLabelEXT = LoadFunction<glGetObjectLabelEXT>();
            _glInsertEventMarkerEXT = LoadFunction<glInsertEventMarkerEXT>();
            _glPushGroupMarkerEXT = LoadFunction<glPushGroupMarkerEXT>();
            _glPopGroupMarkerEXT = LoadFunction<glPopGroupMarkerEXT>();
            _glDrawArraysInstancedEXT = LoadFunction<glDrawArraysInstancedEXT>();
            _glDrawElementsInstancedEXT = LoadFunction<glDrawElementsInstancedEXT>();
            _glPolygonOffsetClampEXT = LoadFunction<glPolygonOffsetClampEXT>();
            _glRasterSamplesEXT = LoadFunction<glRasterSamplesEXT>();
            _glUseShaderProgramEXT = LoadFunction<glUseShaderProgramEXT>();
            _glActiveProgramEXT = LoadFunction<glActiveProgramEXT>();
            _glCreateShaderProgramEXT = LoadFunction<glCreateShaderProgramEXT>();
            _glWindowRectanglesEXT = LoadFunction<glWindowRectanglesEXT>();
            _glApplyFramebufferAttachmentCMAAINTEL = LoadFunction<glApplyFramebufferAttachmentCMAAINTEL>();
            _glBeginPerfQueryINTEL = LoadFunction<glBeginPerfQueryINTEL>();
            _glCreatePerfQueryINTEL = LoadFunction<glCreatePerfQueryINTEL>();
            _glDeletePerfQueryINTEL = LoadFunction<glDeletePerfQueryINTEL>();
            _glEndPerfQueryINTEL = LoadFunction<glEndPerfQueryINTEL>();
            _glGetFirstPerfQueryIdINTEL = LoadFunction<glGetFirstPerfQueryIdINTEL>();
            _glGetNextPerfQueryIdINTEL = LoadFunction<glGetNextPerfQueryIdINTEL>();
            _glGetPerfCounterInfoINTEL = LoadFunction<glGetPerfCounterInfoINTEL>();
            _glGetPerfQueryDataINTEL = LoadFunction<glGetPerfQueryDataINTEL>();
            _glGetPerfQueryIdByNameINTEL = LoadFunction<glGetPerfQueryIdByNameINTEL>();
            _glGetPerfQueryInfoINTEL = LoadFunction<glGetPerfQueryInfoINTEL>();
            _glBlendBarrierKHR = LoadFunction<glBlendBarrierKHR>();
            _glGetTextureHandleNV = LoadFunction<glGetTextureHandleNV>();
            _glGetTextureSamplerHandleNV = LoadFunction<glGetTextureSamplerHandleNV>();
            _glMakeTextureHandleResidentNV = LoadFunction<glMakeTextureHandleResidentNV>();
            _glMakeTextureHandleNonResidentNV = LoadFunction<glMakeTextureHandleNonResidentNV>();
            _glGetImageHandleNV = LoadFunction<glGetImageHandleNV>();
            _glMakeImageHandleResidentNV = LoadFunction<glMakeImageHandleResidentNV>();
            _glMakeImageHandleNonResidentNV = LoadFunction<glMakeImageHandleNonResidentNV>();
            _glUniformHandleui64NV = LoadFunction<glUniformHandleui64NV>();
            _glUniformHandleui64vNV = LoadFunction<glUniformHandleui64vNV>();
            _glProgramUniformHandleui64NV = LoadFunction<glProgramUniformHandleui64NV>();
            _glProgramUniformHandleui64vNV = LoadFunction<glProgramUniformHandleui64vNV>();
            _glIsTextureHandleResidentNV = LoadFunction<glIsTextureHandleResidentNV>();
            _glIsImageHandleResidentNV = LoadFunction<glIsImageHandleResidentNV>();
            _glBlendParameteriNV = LoadFunction<glBlendParameteriNV>();
            _glBlendBarrierNV = LoadFunction<glBlendBarrierNV>();
            _glBeginConditionalRenderNV = LoadFunction<glBeginConditionalRenderNV>();
            _glEndConditionalRenderNV = LoadFunction<glEndConditionalRenderNV>();
            _glSubpixelPrecisionBiasNV = LoadFunction<glSubpixelPrecisionBiasNV>();
            _glConservativeRasterParameteriNV = LoadFunction<glConservativeRasterParameteriNV>();
            _glFragmentCoverageColorNV = LoadFunction<glFragmentCoverageColorNV>();
            _glCoverageModulationTableNV = LoadFunction<glCoverageModulationTableNV>();
            _glGetCoverageModulationTableNV = LoadFunction<glGetCoverageModulationTableNV>();
            _glCoverageModulationNV = LoadFunction<glCoverageModulationNV>();
            _glUniform1i64NV = LoadFunction<glUniform1i64NV>();
            _glUniform2i64NV = LoadFunction<glUniform2i64NV>();
            _glUniform3i64NV = LoadFunction<glUniform3i64NV>();
            _glUniform4i64NV = LoadFunction<glUniform4i64NV>();
            _glUniform1i64vNV = LoadFunction<glUniform1i64vNV>();
            _glUniform2i64vNV = LoadFunction<glUniform2i64vNV>();
            _glUniform3i64vNV = LoadFunction<glUniform3i64vNV>();
            _glUniform4i64vNV = LoadFunction<glUniform4i64vNV>();
            _glUniform1ui64NV = LoadFunction<glUniform1ui64NV>();
            _glUniform2ui64NV = LoadFunction<glUniform2ui64NV>();
            _glUniform3ui64NV = LoadFunction<glUniform3ui64NV>();
            _glUniform4ui64NV = LoadFunction<glUniform4ui64NV>();
            _glUniform1ui64vNV = LoadFunction<glUniform1ui64vNV>();
            _glUniform2ui64vNV = LoadFunction<glUniform2ui64vNV>();
            _glUniform3ui64vNV = LoadFunction<glUniform3ui64vNV>();
            _glUniform4ui64vNV = LoadFunction<glUniform4ui64vNV>();
            _glGetUniformi64vNV = LoadFunction<glGetUniformi64vNV>();
            _glProgramUniform1i64NV = LoadFunction<glProgramUniform1i64NV>();
            _glProgramUniform2i64NV = LoadFunction<glProgramUniform2i64NV>();
            _glProgramUniform3i64NV = LoadFunction<glProgramUniform3i64NV>();
            _glProgramUniform4i64NV = LoadFunction<glProgramUniform4i64NV>();
            _glProgramUniform1i64vNV = LoadFunction<glProgramUniform1i64vNV>();
            _glProgramUniform2i64vNV = LoadFunction<glProgramUniform2i64vNV>();
            _glProgramUniform3i64vNV = LoadFunction<glProgramUniform3i64vNV>();
            _glProgramUniform4i64vNV = LoadFunction<glProgramUniform4i64vNV>();
            _glProgramUniform1ui64NV = LoadFunction<glProgramUniform1ui64NV>();
            _glProgramUniform2ui64NV = LoadFunction<glProgramUniform2ui64NV>();
            _glProgramUniform3ui64NV = LoadFunction<glProgramUniform3ui64NV>();
            _glProgramUniform4ui64NV = LoadFunction<glProgramUniform4ui64NV>();
            _glProgramUniform1ui64vNV = LoadFunction<glProgramUniform1ui64vNV>();
            _glProgramUniform2ui64vNV = LoadFunction<glProgramUniform2ui64vNV>();
            _glProgramUniform3ui64vNV = LoadFunction<glProgramUniform3ui64vNV>();
            _glProgramUniform4ui64vNV = LoadFunction<glProgramUniform4ui64vNV>();
            _glGetInternalformatSampleivNV = LoadFunction<glGetInternalformatSampleivNV>();
            _glGenPathsNV = LoadFunction<glGenPathsNV>();
            _glDeletePathsNV = LoadFunction<glDeletePathsNV>();
            _glIsPathNV = LoadFunction<glIsPathNV>();
            _glPathCommandsNV = LoadFunction<glPathCommandsNV>();
            _glPathCoordsNV = LoadFunction<glPathCoordsNV>();
            _glPathSubCommandsNV = LoadFunction<glPathSubCommandsNV>();
            _glPathSubCoordsNV = LoadFunction<glPathSubCoordsNV>();
            _glPathStringNV = LoadFunction<glPathStringNV>();
            _glPathGlyphsNV = LoadFunction<glPathGlyphsNV>();
            _glPathGlyphRangeNV = LoadFunction<glPathGlyphRangeNV>();
            _glWeightPathsNV = LoadFunction<glWeightPathsNV>();
            _glCopyPathNV = LoadFunction<glCopyPathNV>();
            _glInterpolatePathsNV = LoadFunction<glInterpolatePathsNV>();
            _glTransformPathNV = LoadFunction<glTransformPathNV>();
            _glPathParameterivNV = LoadFunction<glPathParameterivNV>();
            _glPathParameteriNV = LoadFunction<glPathParameteriNV>();
            _glPathParameterfvNV = LoadFunction<glPathParameterfvNV>();
            _glPathParameterfNV = LoadFunction<glPathParameterfNV>();
            _glPathDashArrayNV = LoadFunction<glPathDashArrayNV>();
            _glPathStencilFuncNV = LoadFunction<glPathStencilFuncNV>();
            _glPathStencilDepthOffsetNV = LoadFunction<glPathStencilDepthOffsetNV>();
            _glStencilFillPathNV = LoadFunction<glStencilFillPathNV>();
            _glStencilStrokePathNV = LoadFunction<glStencilStrokePathNV>();
            _glStencilFillPathInstancedNV = LoadFunction<glStencilFillPathInstancedNV>();
            _glStencilStrokePathInstancedNV = LoadFunction<glStencilStrokePathInstancedNV>();
            _glPathCoverDepthFuncNV = LoadFunction<glPathCoverDepthFuncNV>();
            _glCoverFillPathNV = LoadFunction<glCoverFillPathNV>();
            _glCoverStrokePathNV = LoadFunction<glCoverStrokePathNV>();
            _glCoverFillPathInstancedNV = LoadFunction<glCoverFillPathInstancedNV>();
            _glCoverStrokePathInstancedNV = LoadFunction<glCoverStrokePathInstancedNV>();
            _glGetPathParameterivNV = LoadFunction<glGetPathParameterivNV>();
            _glGetPathParameterfvNV = LoadFunction<glGetPathParameterfvNV>();
            _glGetPathCommandsNV = LoadFunction<glGetPathCommandsNV>();
            _glGetPathCoordsNV = LoadFunction<glGetPathCoordsNV>();
            _glGetPathDashArrayNV = LoadFunction<glGetPathDashArrayNV>();
            _glGetPathMetricsNV = LoadFunction<glGetPathMetricsNV>();
            _glGetPathMetricRangeNV = LoadFunction<glGetPathMetricRangeNV>();
            _glGetPathSpacingNV = LoadFunction<glGetPathSpacingNV>();
            _glIsPointInFillPathNV = LoadFunction<glIsPointInFillPathNV>();
            _glIsPointInStrokePathNV = LoadFunction<glIsPointInStrokePathNV>();
            _glGetPathLengthNV = LoadFunction<glGetPathLengthNV>();
            _glPointAlongPathNV = LoadFunction<glPointAlongPathNV>();
            _glMatrixLoad3x2fNV = LoadFunction<glMatrixLoad3x2fNV>();
            _glMatrixLoad3x3fNV = LoadFunction<glMatrixLoad3x3fNV>();
            _glMatrixLoadTranspose3x3fNV = LoadFunction<glMatrixLoadTranspose3x3fNV>();
            _glMatrixMult3x2fNV = LoadFunction<glMatrixMult3x2fNV>();
            _glMatrixMult3x3fNV = LoadFunction<glMatrixMult3x3fNV>();
            _glMatrixMultTranspose3x3fNV = LoadFunction<glMatrixMultTranspose3x3fNV>();
            _glStencilThenCoverFillPathNV = LoadFunction<glStencilThenCoverFillPathNV>();
            _glStencilThenCoverStrokePathNV = LoadFunction<glStencilThenCoverStrokePathNV>();
            _glStencilThenCoverFillPathInstancedNV = LoadFunction<glStencilThenCoverFillPathInstancedNV>();
            _glStencilThenCoverStrokePathInstancedNV = LoadFunction<glStencilThenCoverStrokePathInstancedNV>();
            _glPathGlyphIndexRangeNV = LoadFunction<glPathGlyphIndexRangeNV>();
            _glPathGlyphIndexArrayNV = LoadFunction<glPathGlyphIndexArrayNV>();
            _glPathMemoryGlyphIndexArrayNV = LoadFunction<glPathMemoryGlyphIndexArrayNV>();
            _glProgramPathFragmentInputGenNV = LoadFunction<glProgramPathFragmentInputGenNV>();
            _glGetProgramResourcefvNV = LoadFunction<glGetProgramResourcefvNV>();
            _glFramebufferSampleLocationsfvNV = LoadFunction<glFramebufferSampleLocationsfvNV>();
            _glNamedFramebufferSampleLocationsfvNV = LoadFunction<glNamedFramebufferSampleLocationsfvNV>();
            _glResolveDepthValuesNV = LoadFunction<glResolveDepthValuesNV>();
            _glViewportSwizzleNV = LoadFunction<glViewportSwizzleNV>();
            _glFramebufferTextureMultiviewOVR = LoadFunction<glFramebufferTextureMultiviewOVR>();


            UnLoadLib();
        }
    }
}
