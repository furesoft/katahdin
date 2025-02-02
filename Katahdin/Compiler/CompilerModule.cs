using System;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;

using Katahdin.CodeTree;

namespace Katahdin.Compiler
{
    public class CompilerModule
    {
        private ModuleBuilder moduleBuilder;
        
        public CompilerModule()
        {
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("runtime"),
                AssemblyBuilderAccess.Run);
            
            moduleBuilder = assemblyBuilder.DefineDynamicModule("runtime");
        }
        
        public ModuleBuilder ModuleBuilder
        {
            get
            {
                return moduleBuilder;
            }
        }

        public MethodInfo CompileDynamicMethod(string name, CodeTreeNode codeTreeNode)
        {
            Block generator = new Block();
            codeTreeNode.EmitRun(generator);

            DynamicMethod method = new DynamicMethod(
                name,
                typeof(object), 
                new Type[]{typeof(Runtime), typeof(IScope)},
                moduleBuilder);

            generator.Build(new DynamicMethodProxy(method));

            return method;
        }
    }
}
