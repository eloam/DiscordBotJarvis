using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Interfaces;

namespace DiscordBotJarvis.Core
{
    public class XmlCallbackSerializer : XmlSerializer
    {
        public XmlCallbackSerializer(Type type) : base(type)
        {
        }

        public XmlCallbackSerializer(XmlTypeMapping xmlTypeMapping) : base(xmlTypeMapping)
        {
        }

        public XmlCallbackSerializer(Type type, string defaultNamespace) : base(type, defaultNamespace)
        {
        }

        public XmlCallbackSerializer(Type type, Type[] extraTypes) : base(type, extraTypes)
        {
        }

        public XmlCallbackSerializer(Type type, XmlAttributeOverrides overrides) : base(type, overrides)
        {
        }

        public XmlCallbackSerializer(Type type, XmlRootAttribute root) : base(type, root)
        {
        }

        public XmlCallbackSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes,
            XmlRootAttribute root, string defaultNamespace) : base(type, overrides, extraTypes, root, defaultNamespace)
        {
        }

        public XmlCallbackSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes,
            XmlRootAttribute root, string defaultNamespace, string location)
            : base(type, overrides, extraTypes, root, defaultNamespace, location)
        {
        }

        public new object Deserialize(Stream stream)
        {
            var result = base.Deserialize(stream);

            CheckForDeserializationCallbacks(result);

            return result;
        }

        public new object Deserialize(TextReader textReader)
        {
            var result = base.Deserialize(textReader);

            CheckForDeserializationCallbacks(result);

            return result;
        }

        public new object Deserialize(XmlReader xmlReader)
        {
            var result = base.Deserialize(xmlReader);

            CheckForDeserializationCallbacks(result);

            return result;
        }

        public new object Deserialize(XmlSerializationReader reader)
        {
            var result = base.Deserialize(reader);

            CheckForDeserializationCallbacks(result);

            return result;
        }

        public new object Deserialize(XmlReader xmlReader, string encodingStyle)
        {
            var result = base.Deserialize(xmlReader, encodingStyle);

            CheckForDeserializationCallbacks(result);

            return result;
        }

        public new object Deserialize(XmlReader xmlReader, XmlDeserializationEvents events)
        {
            var result = base.Deserialize(xmlReader, events);

            CheckForDeserializationCallbacks(result);

            return result;
        }

        public new object Deserialize(XmlReader xmlReader, string encodingStyle, XmlDeserializationEvents events)
        {
            var result = base.Deserialize(xmlReader, encodingStyle, events);

            CheckForDeserializationCallbacks(result);

            return result;
        }

        private void CheckForDeserializationCallbacks(object result)
        {
            if (!result.GetType().IsCollection())
            {
                if (result is IXmlDeserializationCallback)
                {
                    var deserializedCallback = result as IXmlDeserializationCallback;
                    deserializedCallback?.OnXmlDeserialization(this);
                }
                else if (result is IEnumerable<IXmlFeedbacksDeserializationCallback>)
                {
                    IEnumerable<IXmlFeedbacksDeserializationCallback> resultList = result as IEnumerable<IXmlFeedbacksDeserializationCallback>;
                    foreach (IXmlFeedbacksDeserializationCallback deserializedCallback in resultList)
                    {
                        Feedback[] feedbacksCommandSet = deserializedCallback?.Feedbacks;

                        if (feedbacksCommandSet == null) continue;
                        foreach (Feedback currentFeedback in feedbacksCommandSet)
                        {
                            if (currentFeedback == null) continue;

                            if (currentFeedback is Sentence) ((Sentence)currentFeedback)?.OnXmlDeserialization(this);
                            if (currentFeedback is SentenceFile) ((SentenceFile)currentFeedback)?.OnXmlDeserialization(this);
                            if (currentFeedback is Service) ((Service)currentFeedback)?.OnXmlDeserialization(this);
                        }
                    }
                }
            }

            

        }
    }
}
