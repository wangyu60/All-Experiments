using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TextTemplate_try
{
    class Program
    {
        static void Main(string[] args)
        {
            var codeGen = new TestCodeGen();
            //var test = @"$""{string}""";
            codeGen.Do();
            Console.ReadLine();
        }
    }

    class TestCodeGen
    {

        public void Do()
        {
            foreach (var methodInfo in typeof(ASM.AS.FeederManager.Server.ServiceContracts.IClientBusiness).GetMethods())
            {
                //var returnType = GetMethodReturnType(methodInfo);
                var paramStr = GetLogParamString(methodInfo);
                var errMsg = GetMethodErrorMessage(methodInfo);
                Console.WriteLine(paramStr);
                Console.WriteLine(errMsg);
            }
        }

        private string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type == typeof(void) ? "void" : type.Name;
            }

            Type[] paramArry = type.GetGenericArguments();
            int paramLen = paramArry.Length;
            string returnTypeGenericTypeName = type.Name.Replace("`" + paramLen, "");
            switch (paramLen)
            {
                case 1:
                    return $"{returnTypeGenericTypeName}<{type.GetGenericArguments()[0].Name}>";
                case 2:
                    return
                        $"{returnTypeGenericTypeName}<{type.GetGenericArguments()[0].Name}, {type.GetGenericArguments()[1].Name}>";
                case 3:
                    return
                        $"{returnTypeGenericTypeName}<{type.GetGenericArguments()[0].Name}, {type.GetGenericArguments()[1].Name}, {type.GetGenericArguments()[2].Name}>";
                case 4:
                    return
                        $"{returnTypeGenericTypeName}<{type.GetGenericArguments()[0].Name}, {type.GetGenericArguments()[1].Name}, {type.GetGenericArguments()[2].Name}, {type.GetGenericArguments()[3].Name}>";
                default:
                    return string.Empty;
            }
        }

        private string GetMethodReturnType(MethodInfo method)
        {
            return GetTypeName(method.ReturnType);
        }

        private string GetMethodParameterList(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(GetTypeName(parameters[i].ParameterType));
                sb.Append(" ");
                sb.Append(parameters[i].Name);
                if (i != parameters.Length - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }

        private string GetMethodParameterValueList(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(parameters[i].Name);
                if (i != parameters.Length - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }

        private string GetMethodParameterValueStr(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.AppendFormat("JsonConvert.SerializeObject({0})", parameters[i].Name);
                if (i != parameters.Length - 1)
                    sb.Append(",");
            }
            return sb.ToString();
        }

        private string GetMethodParameterNameStr(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.AppendFormat("{0}:{{{1}}}", parameters[i].Name, i);
                if (i != parameters.Length - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }

        private string myGet(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var builder = new StringBuilder();
            builder.AppendFormat(@"$""");
            for (int i = 0; i < parameters.Length; i++)
            {
                builder.AppendFormat("{0}:{{JsonConvert.SerializeObject({0})}}", parameters[i].Name);
                //builder.AppendFormat("JsonConvert.SerializeObject({0})", parameters[i].Name);

                if (i != parameters.Length - 1)
                    builder.Append(", ");
            }
            builder.Append(@".""");
            return builder.ToString();

        }

        private string GetMethodErrorMessage(MethodInfo method)
        {
            string message;
            string paramNameStr = GetMethodParameterValueList(method);
            if (string.IsNullOrEmpty(paramNameStr))
            {
                message = $"\"{method.Name}() has error.\"";
            }
            else
            {
                //string name = GetMethodParameterNameStr(method);
                //string value = GetMethodParameterValueStr(method);
                //message = string.Format("string.Format(\"{0}({1}) has errors.{2}.\",{3})", method.Name, paramNameStr, name, value);
                
                var builder = new StringBuilder();
                builder.AppendFormat(@"$""{0}({1}) has errors. ", method.Name, paramNameStr);
                AppendParams(method, builder);
                

                builder.Append(@".""");

                message = builder.ToString();

            }
            return message;
        }

        private string GetLogParamString(MethodInfo method)
        {
            string paramNameStr = GetMethodParameterValueList(method);
            if (string.IsNullOrEmpty(paramNameStr)) return string.Empty;

            //string name = GetMethodParameterNameStr(method);
            //string value = GetMethodParameterValueStr(method);
            //message = string.Format("Logger.Debug(string.Format(\"{0}.\", {1}));", name, value);

            
            var builder = new StringBuilder();
            builder.AppendFormat(@"Logger.Debug($""");
            AppendParams(method, builder);
            builder.Append(@"."");");

            return builder.ToString();

        }

        private static void AppendParams(MethodInfo method, StringBuilder builder)
        {
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                builder.AppendFormat("{0}:{{JsonConvert.SerializeObject({0})}}", parameters[i].Name);

                if (i != parameters.Length - 1)
                    builder.Append(", ");
            }
        }
    }
}
