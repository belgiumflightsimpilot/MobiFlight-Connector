﻿using MobiFlight.BrowserMessages.Incoming.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MobiFlight.Modifier
{
    public class ModifierList : IXmlSerializable, ICloneable
    {
        
        protected List<ModifierBase> modifiers = new List<ModifierBase>();

        [JsonProperty(ItemConverterType = typeof(ModifierBaseConverter))]
        public List<ModifierBase> Items { get { return modifiers; } }

        [JsonIgnore]
        public Transformation Transformation
        {
            get
            {
                foreach (ModifierBase modifier in modifiers)
                {
                    if (modifier is Transformation)
                        return modifier as Transformation;
                }

                var t = new Transformation();
                modifiers.Add(t);
                return t;
            }
        }
        [JsonIgnore]
        public Comparison Comparison
        {
            get
            {
                foreach (ModifierBase modifier in modifiers)
                {
                    if (modifier is Comparison)
                        return modifier as Comparison;
                }

                var c = new Comparison();
                modifiers.Add(c);
                return c;
            }
        }
        [JsonIgnore]
        public Interpolation Interpolation
        {
            get
            {
                foreach (ModifierBase modifier in modifiers)
                {
                    if (modifier is Interpolation)
                        return modifier as Interpolation;
                }

                var i = new Interpolation();
                modifiers.Add(i);
                return i;
            }
        }

        public override bool Equals(object obj)
        {
            var result = obj != null && obj is ModifierList && (obj as ModifierList).Items.Count == Items.Count;
            if (!result) return result;

            for(var i = 0; i < modifiers.Count; i++)
            {
                if (!modifiers[i].Equals((obj as ModifierList).Items[i]))
                    return false;
            }

            return true;
        }

        public void ReadXml(XmlReader reader)
        {
            // we have an empty tag
            if (reader.LocalName=="modifiers" && reader.IsEmptyElement)
            {
                // advance to the next node
                reader.Read();
                return;
            }

            do
            {
                // advance to the next node
                reader.Read();
                ReadModifiers(reader);
            } while (reader.LocalName != "modifiers");

            // are still on the closing tag
            if (reader.LocalName == "modifiers" && reader.NodeType == XmlNodeType.EndElement)
            {
                // advance to the next node
                reader.Read();
                return;
            }
        }

        protected virtual void ReadModifiers(XmlReader reader)
        {
            switch (reader.LocalName)
            {
                case "transformation":
                    var t = new Transformation();
                    t.ReadXml(reader);
                    modifiers.Add(t);
                    break;

                case "blink":
                    var b = new Blink();
                    b.ReadXml(reader);
                    modifiers.Add(b);
                    break;

                case "comparison":
                    var c = new Comparison();
                    c.ReadXml(reader);
                    modifiers.Add(c);
                    break;

                case "interpolation":
                    var i = new Interpolation();
                    i.ReadXml(reader);
                    modifiers.Add(i);
                    break;

                case "padding":
                    var p = new Padding();
                    p.ReadXml(reader);
                    modifiers.Add(p);
                    break;

                case "substring":
                    var s = new Substring();
                    s.ReadXml(reader);
                    modifiers.Add(s);
                    break;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("modifiers");
            foreach (var modifier in modifiers)
            {
                modifier.WriteXml(writer);
            }
            writer.WriteEndElement();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public virtual object Clone()
        {
            var clone = new ModifierList();
            foreach (var modifier in modifiers)
            {
                clone.Items.Add(modifier.Clone() as ModifierBase);
            }

            return clone;
        }

        public bool ContainsModifier(ModifierBase modifier)
        {
            return modifiers.Find(m => m.Equals(modifier)) != null;
        }
    }
}
