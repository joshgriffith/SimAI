using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimAI.Core.Extensions;
using SimAI.Core.Intent;
using SimAI.Core.Templates;
using SimAI.Core.Templates.Nodes;
using SimAI.Core.Tokenization;

namespace SimAI.Core.Entities {
    public class EntityMatcher {

        /// <summary>
        /// Attempts to extract entities from tokens to match parameters using a template
        /// </summary>7
        public List<EntityToken> Map(List<EntityParameter> parameters, Template template, TokenSequence input) {
            var entities = new List<EntityToken>();

            if (!parameters.Any())
                return null;

            var index = 0;

            foreach (var token in input.Tokens) {
                var node = template.Nodes[index];

                if (node is TemplateExpression intent) {

                    var parameter = intent.ParameterName.IsEmpty() ? parameters[entities.Count] : parameters.First(each => each.Name == intent.ParameterName);
                    var entity = new EntityToken(parameter.Name);

                    if (parameter.Type == typeof(int))
                        entity.Value = int.Parse(token.Value);
                    else {
                        entity.Value = token.Value;

                        if (token.IsEntityBegin(intent.Value)) {
                            foreach (var sibling in input.Tokens.Skip(index + 1)) {
                                if (sibling.HasTag(intent.Value))
                                    entity.Value += sibling.Value;
                                else
                                    break;
                            }
                        }
                    }

                    entities.Add(entity);

                    if (entities.Count == parameters.Count)
                        return entities;
                }

                index += 1;
            }

            return null;
        }
    }
}