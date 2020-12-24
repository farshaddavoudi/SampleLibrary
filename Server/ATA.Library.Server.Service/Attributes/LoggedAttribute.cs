using AspectInjector.Broker;
using ATA.Library.Shared.Service.Extensions;
using System;
using System.Diagnostics;

namespace ATA.Library.Server.Service.Attributes
{
    [Aspect(Scope.Global)]
    [Injection(typeof(LoggedAttribute))]
    public sealed class LoggedAttribute : Attribute
    {
        [Advice(Kind.Around, Targets = Target.Method)]
        public object? Trace(
            [Argument(Source.Type)] Type type,
            [Argument(Source.Name)] string name,
            [Argument(Source.Target)] Func<object[], object> methodDelegate,
            [Argument(Source.Arguments)] object[] args)
        {
            var argsSerialized = args.SerializeToJson();
            var logger = Serilog.Log.ForContext<Type>();
            logger.Information($"[{DateTime.Now}] Method {type.Name}.{name} started with Args: {argsSerialized}");

            #region Without Try Catch block (Assuming CustomException Middleware will cover our needs)
            var sw = Stopwatch.StartNew();
#pragma warning disable CS8604 // Possible null reference argument.
            var result = methodDelegate(args);
#pragma warning restore CS8604 // Possible null reference argument.
            sw.Stop();
            var resultSerialized = result.SerializeToJson();
            logger.Information($"[{DateTime.Now}] Method {type.Name}.{name} finished in {sw.ElapsedMilliseconds} ms with Result: {resultSerialized}");
            return result;
            #endregion

            #region Having Try Catch block for capturing Exceptions
            //try
            //{
            //    var sw = Stopwatch.StartNew();
            //    var result = methodDelegate(args);
            //    sw.Stop();
            //    var resultSerialized = result == null ? null : JsonConvert.SerializeObject(result);
            //    logger.Information($"[{DateTime.Now}] Method {type.Name}.{name} finished in {sw.ElapsedMilliseconds} ms with Result: {resultSerialized}");
            //    return result;
            //}
            //catch (Exception e)
            //{
            //    logger.Error($"[{DateTime.Now}] Method {type.Name}.{name} encountered error: {e.Message}");
            //    throw;
            //}
            #endregion
        }
    }
}
