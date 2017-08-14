#pragma warning disable 1591
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Reflection;
using System.Collections.Generic;

IofeX2QJE3YknRllLqrZ
l

namespace $namespace
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

$features
        #endregion

        #region Extensions

$extensions
        #endregion

        /// <summary>
        /// Init library for everything defined.
        /// </summary>
        public static void Load()
        {
            LoadLib();

$functions_loading

            UnLoadLib();
        }
    }
}
