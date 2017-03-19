using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Interfaces;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
{
    [XmlInclude(typeof(Sentence))]
    [XmlInclude(typeof(SentenceFile))]
    [XmlInclude(typeof(Service))]
    public class CommandSet : IXmlDeserializationCallback
    {
        // Variables
        private string[] _regexTriggers;
        private List<string[]> _triggersDefinedByUser;

        // Property
        public Feedback[] Feedbacks { get; set; }

        [XmlIgnore]
        public Regex[] TriggersRegex { get; set; }

        [XmlArray("Triggers")]
        [XmlArrayItem("Condition")]
        public List<string[]> TriggersDefinedByUser {
            get { return _triggersDefinedByUser; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(TriggersDefinedByUser), "La balise est manquante.");

                if (value.Count > 0)
                    SetRegexTriggers(value);

                _triggersDefinedByUser = value;
            }
        }

        [DefaultValue(true)]
        public bool BotMentionRequired { get; set; }

        [XmlIgnore]
        public bool IsListKeywordsEmpty => TriggersDefinedByUser?.Count == 0;

        // Contructors
        public CommandSet()
        {
            TriggersDefinedByUser = new List<string[]>();
            BotMentionRequired = true;
        }

        private CommandSet(Feedback[] feedbacks, bool botMentionRequired = true)
        {
            Feedbacks = feedbacks;
            BotMentionRequired = botMentionRequired;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> triggers, bool botMentionRequired = true) : this(feedbacks, botMentionRequired)
        {
            if (triggers == null)
                throw new ArgumentNullException(nameof(triggers));

            TriggersDefinedByUser = triggers;
        }


        private void SetRegexTriggers(IEnumerable<string[]> triggersList)
        {
            // On réalise une concaténation afin de regrouper en une seul liste toutes les opérateurs 
            // logiques de type OU dans une seule et même chaine de caractères.
            List<Regex> result = new List<Regex>();
            foreach (string[] condition in triggersList)
            {
                StringBuilder conditionConcatenate = new StringBuilder().Append("(^| )(");
                for (int item = 0; item < condition.Length; item++)
                {
                    conditionConcatenate.Append(condition[item]);
                    conditionConcatenate.Append(item < condition.Length - 1 ? "|" : @")($|\.| )");
                }

                Regex rgx = new Regex(conditionConcatenate.ToString(),
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                result.Add(rgx);
            }
            TriggersRegex = result.ToArray();
        }

        public void OnXmlDeserialization(object sender)
        {
            if (TriggersDefinedByUser == null)
                throw new ArgumentNullException(nameof(TriggersDefinedByUser), "La balise est manquante.");

            SetRegexTriggers(TriggersDefinedByUser);

            if (Feedbacks == null) return;
            foreach (Feedback feedback in Feedbacks)
            {
                if (feedback == null) continue;

                if (feedback is Sentence) ((Sentence)feedback)?.OnXmlDeserialization(this);
                else if (feedback is SentenceFile) ((SentenceFile)feedback)?.OnXmlDeserialization(this);
                else if (feedback is Service) ((Service)feedback)?.OnXmlDeserialization(this);
            }
        }
    }
}
