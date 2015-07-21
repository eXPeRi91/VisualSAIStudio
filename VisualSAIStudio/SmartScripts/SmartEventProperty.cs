﻿using DynamicTypeDescriptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSAIStudio.SmartScripts
{

    


    public class SmartElementProperty
    {
        protected SmartElement element;
        public DynamicCustomTypeDescriptor m_dctd = null;

        [CategoryAttribute("Parameters"),
        Id(0, 0)]
        public int pram1 {
            get { return element.parameters[0].GetValue(); }
            set { element.UpdateParams(0, value); }
        }

        [CategoryAttribute("Parameters"),
        Id(1, 0)]
        public int pram2
        {
            get { return element.parameters[1].GetValue(); }
            set { element.UpdateParams(1, value); }
        }

        [CategoryAttribute("Parameters")]
        [Id(2, 0)]
        public int pram3
        {
            get { return element.parameters[2].GetValue(); }
            set { element.UpdateParams(2, value); }
        }

        [CategoryAttribute("Parameters")]
        [Id(3, 0)]
        public int pram4
        {
            get { return element.parameters[3].GetValue(); }
            set { element.UpdateParams(3, value); }
        }

        [CategoryAttribute("Parameters")]
        [Id(4, 0)]
        public int pram5
        {
            get { return element.parameters[4].GetValue(); }
            set { element.UpdateParams(4, value); }
        }

        [Editor(typeof(StandardValueEditor), typeof(UITypeEditor))]
        [CategoryAttribute("Parameters")]
        [Id(5, 0)]
        public int pram6
        {
            get { return element.parameters[5].GetValue(); }
            set { element.UpdateParams(5, value); }
        }

        public SmartElementProperty(SmartElement element)
        {
            this.element = element;
            m_dctd = ProviderInstaller.Install(this);
            m_dctd.PropertySortOrder = CustomSortOrder.AscendingById;
            TypeDescriptor.Refresh(this);
            Parameter[] parameters = element.parameters;
            for (int i = 0; i < 6; ++i)
            {
                CustomPropertyDescriptor property = m_dctd.GetProperty("pram" + (i + 1));
                Init(property, parameters[i]);
            }
        }
        protected void Init(CustomPropertyDescriptor property, Parameter parameter)
        {
            if (parameter is NullParameter)
                property.SetIsBrowsable(false);
            else
            {
                property.SetDescription(parameter.description);
                property.SetDisplayName(parameter.name);
                if (parameter is SwitchParameter)
                    ((SwitchParameter)parameter).AddValuesToProperty(property);
                if (parameter is FlagParameter)
                {
                    property.AddAttribute(new EditorAttribute(typeof(StandardValueEditor), typeof(UITypeEditor)));
                    property.PropertyFlags |= PropertyFlags.IsFlag;
                }
            }
        }
    }

    public class SmartEventProperty : SmartElementProperty
    {
        [ReadOnly(true),
        CategoryAttribute("Event"),
        DisplayName("Event name")]
        public string event_name { get; set; }

        [CategoryAttribute("Event"),
        DisplayName("Phasemask")]
        public SmartPhaseMask phasemask { 
            get 
            {
                return ((SmartEvent)element).phasemask;
            }
            set
            {
                ((SmartEvent)element).phasemask = value; element.Invalide();
            } 
        }

        [CategoryAttribute("Event"),
        DisplayName("Flags")]
        public SmartEventFlag flags
        { 
            get 
            {
                return ((SmartEvent)element).flags;
            }
            set
            {
                ((SmartEvent)element).flags = value; element.Invalide();
            } 
        }
        

        [CategoryAttribute("Event"),
        DisplayName("Chance")]
        public int chance 
        {
            get
            {
                return ((SmartEvent)element).chance;
            }
            set 
            {
                if (value > 100 )
                    value = 100;
                else if (value < 0)
                    value = 0;
                ((SmartEvent)element).chance = value; element.Invalide();
            }
        }

        public SmartEventProperty(SmartEvent ev) : base (ev)
        {
            event_name = ev.GetType().Name;
        }
    }

    public class SmartActionProperty : SmartElementProperty
    {
        SmartAction action;

        [ReadOnly(true),
        CategoryAttribute("Action"),
        DisplayName("Action name")]
        public string action_name { get; set; }

        [CategoryAttribute("Target Position"),
         DisplayName("X")]
        public float target_x
        {
            get { return ((SmartAction)element).target.position[0]; }
            set { action.target.position[0] = value; action.Invalide(); }
        }

        [CategoryAttribute("Target Position"),
        DisplayName("Y")]
        public float target_y
        {
            get { return ((SmartAction)element).target.position[1]; }
            set { action.target.position[1] = value; action.Invalide(); }
        }

        [CategoryAttribute("Target Position")]
        [DisplayName("Z")]
        public float target_z
        {
            get { return ((SmartAction)element).target.position[2]; }
            set { action.target.position[2] = value; action.Invalide(); }
        }

        [CategoryAttribute("Target Position")]
        [DisplayName("O")]
        public float target_o
        {
            get { return ((SmartAction)element).target.position[3]; }
            set { action.target.position[3] = value; action.Invalide(); }
        }



        [CategoryAttribute("Target"),
        Id(0, 0)]
        public int targetpram1
        {
            get { return ((SmartAction)element).target.parameters[0].GetValue(); }
            set { ((SmartAction)element).target.parameters[0].SetValue(value); element.Invalide(); }
        }

        [CategoryAttribute("Target"),
        Id(1, 0)]
        public int targetpram2
        {
            get { return ((SmartAction)element).target.parameters[1].GetValue(); }
            set { ((SmartAction)element).target.parameters[1].SetValue(value); element.Invalide(); }
        }

        [CategoryAttribute("Target")]
        [Id(2, 0)]
        public int targetpram3
        {
            get { return ((SmartAction)element).target.parameters[2].GetValue(); }
            set { ((SmartAction)element).target.parameters[2].SetValue(value); element.Invalide(); }
        }


        public SmartActionProperty(SmartAction action)
            : base(action)
        {
            this.action = action;
            action_name = action.name;
            Parameter[] parameters = action.target.parameters;
            for (int i = 0; i < 3; ++i)
            {
                CustomPropertyDescriptor property = m_dctd.GetProperty("targetpram" + (i + 1));
                Init(property, parameters[i]);
            }
        }

    }

}
