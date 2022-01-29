using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DDocsBackend
{
    internal class RestMethodInfo
    {
        public string RouteName
            => _route.Name;
        public string RouteMethod
            => _route.Method;

        private readonly Route _route;
        private readonly MethodInfo _info;
        private readonly Regex? _routeParamRegex;
        private readonly MatchEvaluator _routeParamEvaluator = new((a) => $"(?<{a.Groups[1].Value}>.+?)");
        private readonly Dictionary<(int index, string name), Type> _parameters = new();
        private readonly Logger _log;

        public RestMethodInfo(Route route, MethodInfo info)
        {
            _log = Logger.GetLogger<RestMethodInfo>();

            _route = route;
            _info = info;

            if (!route.IsRegex)
                _routeParamRegex = ConstructRouteParamRegex(route.Name);

            var parameters = info.GetParameters();

            for (int i = 0; i != parameters.Length; i++)
            {
                var param = parameters[i];
                _parameters.Add((i, param.Name!), param.ParameterType);
            }
        }

        public bool IsMatch(string route, string method)
        {
            if (_route.IsRegex && RouteMethod == method)
                return Regex.IsMatch(_route.Name, route);
            else return _routeParamRegex!.IsMatch(route) && method == RouteMethod;
        }

        public object[]? GetConvertedParameters(string route)
        {
            if (_route.IsRegex)
            {
                var regType = _parameters.FirstOrDefault();
                if (regType.Equals(default(KeyValuePair<string, Type>)))
                    return new object[] { };

                if (regType.Value == typeof(MatchCollection))
                    return new object[] { Regex.Matches(route, _route.Name) };
                else if (regType.Value == typeof(Match))
                    return new object[] { Regex.Match(route, _route.Name) };
                else return new object[] { };
            }

            object[] arr = new object[_parameters.Count];

            var routeParams = GetRouteParams(route);

            foreach (var item in routeParams)
            {
                var rawParam = _parameters.FirstOrDefault(x => x.Key.name == item.Key);

                if (rawParam.Value == null)
                    continue;

                try
                {
                    arr[rawParam.Key.index] = Convert.ChangeType(item.Value, rawParam.Value);
                }
                catch (Exception x)
                {
                    _log.Warn($"{x}", Severity.Rest);
                    return null;
                }
            }

            return arr;
        }

        public Task<RestResult> Execute(object instance, params object[] parameters)
            => (Task<RestResult>?)_info.Invoke(instance, parameters)!;
        public async Task<RestResult> ExecuteAsync(object instance, params object[] parameters)
            => await ((Task<RestResult>?)this._info.Invoke(instance, parameters)!).ConfigureAwait(false);

        private Regex ConstructRouteParamRegex(string route)
        {
            var s = Regex.Escape(route.Replace("{", "C_LBR").Replace("}", "C_RBR")).Replace("C_LBR", "{").Replace("C_RBR", "}");
            var val = Regex.Replace(s, @"{(.+?)}", _routeParamEvaluator);
            return new Regex($"^{val}(?>/|)$".Replace("/", "\\/"));
        }

        private Dictionary<string, string> GetRouteParams(string route)
        {
            if (_routeParamRegex == null)
                return new();

            var matches = _routeParamRegex.Matches(route);

            var dict = new Dictionary<string, string>();

            foreach (var item in _parameters)
            {
                var match = matches.FirstOrDefault(x => x.Groups.ContainsKey(item.Key.name));

                if (match == null)
                    continue;

                dict.Add(item.Key.name, match.Groups[item.Key.name].Value);
            }

            return dict;
        }
    }
}
