using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;
using UnityEngine;
using System.Text;
using System.Linq;
using Zeef;
using TMPro;

namespace Zeef
{
    // http://www.arcturuscollective.com/archives/22
    public class RuntimeCode : SingleInstance<RuntimeCode>
    {
        /// <summary>
        /// Wrap provided code in a method.
        /// </summary>
        public static string Shell(string innerCode)
        {
            return
            "using System;" +
            "using UnityEngine;" +
            "namespace RuntimeCode" +
            "{" +
            "  public static class RuntimeCodeInstance" +
            "   {" +
            "       public static void Execute()" +
            "       {" +
                        innerCode +
            "       }" +
            "   }" +
            "}";
        }

        /// <summary>
        /// Generate assembly from string.
        /// </summary>
        public static Assembly Compile(string code) 
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            // Parameters for provider
            CompilerParameters parameters = new CompilerParameters();

            // Match Unity assembly references
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic)) 
                parameters.ReferencedAssemblies.Add(assembly.Location);
             
            // Generate assembly/.dll in memory
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            // Compile
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.HasErrors) 
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors) 
                    sb.Append($"Error ({error.ErrorNumber}): {error.ErrorText}\n");                
                throw new Exception(sb.ToString());
            }
            else 
            {
                return results.CompiledAssembly;
            }
        }

        /// <summary>
        /// Execute method from provided assembly.
        /// </summary>
        public static object ExecuteMethodFromAssembly(Assembly assembly)  
        {
            Type type = assembly.GetType($"RuntimeCode.RuntimeCodeInstance");
            MethodInfo method = type.GetMethod("Execute");
            return method.Invoke(null, new object[] {});
        }

        /// <summary>
        /// Execute method from provided assembly.
        /// </summary>
        public static object ExecuteMethodFromAssembly(Assembly assembly, string namespaceName, string className, string methodName, bool isStaticClass, object[] arguments)  
        {
            Type type = assembly.GetType($"{namespaceName}.{className}");

            object instance = (!isStaticClass) 
                ? assembly.CreateInstance($"{namespaceName}.{className}")
                : null;

            MethodInfo method = type.GetMethod(methodName);
            return method.Invoke(instance, arguments);
        }
    }
}