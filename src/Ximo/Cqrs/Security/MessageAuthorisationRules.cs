using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Ximo.Cqrs.Security
{
    internal class MessageAuthorisationRules<TMessage> where TMessage : class
    {

        public MessageAuthorisationRules()
        {
            Rules = new List<Type>();
        }
        public void AddAuthorisationRule<TAuthorisationRule>() where TAuthorisationRule : IAuthorizationRule<TMessage>
        {
            var list = Rules.ToList();
            list.Add(typeof(TAuthorisationRule));
            Rules = list;
        }

        public IEnumerable<Type> Rules { get; private set; }
    }
}