using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace DynamicRulesSample
{
    public class Evaluate : IDisposable
    {
        #region Properties

        ScriptOptions _scriptOptions = ScriptOptions.Default;
        Script _script = null;
        bool _preCompiled = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Instanciate Expression Evaluation
        /// </summary>
        /// <param name="Type">Model type (Required for precompile)</param>
        /// <param name="Expression">Expression string (Required for precompile)</param>
        /// <param name="references">Script references</param>
        /// <param name="namespaces">Script namespaces</param>
        public Evaluate(Type Type = null, string Expression = null, List<Assembly> references = null, List<string> namespaces = null)
        {
            // Add References
            if (references == null)
            {
                _scriptOptions = _scriptOptions.AddReferences(typeof(System.Object).GetTypeInfo().Assembly, typeof(System.Linq.Enumerable).GetTypeInfo().Assembly);
            }
            else
            {
                _scriptOptions = _scriptOptions.AddReferences(references.ToArray());
            }

            // Add namespaces
            if (namespaces == null)
            {
                // Default namespaces
                _scriptOptions = _scriptOptions.AddImports("System");
                _scriptOptions = _scriptOptions.AddImports("System.Linq");
                _scriptOptions = _scriptOptions.AddImports("System.Collections.Generic");
            }
            else
            {
                foreach (var n in namespaces)
                {
                    _scriptOptions = _scriptOptions.AddImports(n);
                }
            }

            // Precompile
            if (Type != null && Expression != null)
            {
                _script = CSharpScript.Create(Expression, _scriptOptions, Type);
                _script.Compile();
                _preCompiled = true;
            }
        }

        #endregion

        /// <summary>
        /// Evaluate Expression
        /// <para/>1. Compile and evaluate (null, not null)
        /// <para/>2. Compile and evaluate with model (not null, not null)
        /// <para/>3. Evaluate precompiled script with model (not null, null)
        /// </summary>
        /// <param name="Model">Model to evaluate</param>
        /// <param name="Expression">Expression</param>
        /// <returns></returns>
        public async Task<object> RunAsync(object Model = null, string Expression = null)
        {
            object result = null;

            if (Expression == null)
            {
                if (!_preCompiled)
                {
                    throw new Exception("Script isn't compiled");
                }

                // With model & precompiled
                await _script.RunAsync(Model).ContinueWith(s => result = s.Result.ReturnValue);
            }
            else if (Model != null)
            {
                // With model
                await CSharpScript.EvaluateAsync(Expression, _scriptOptions, Model).ContinueWith(s => result = s.Result);
            }
            else
            {
                // Without model
                await CSharpScript.EvaluateAsync(Expression, _scriptOptions).ContinueWith(s => result = s.Result);
            }

            return result;
        }

        public void Dispose()
        {
            _script = null;
            _preCompiled = false;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
}