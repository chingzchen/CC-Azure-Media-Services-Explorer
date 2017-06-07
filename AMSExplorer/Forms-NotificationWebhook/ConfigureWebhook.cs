//----------------------------------------------------------------------------------------------
//    Copyright 2016 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//---------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.WindowsAzure.MediaServices.Client;


namespace AMSExplorer
{
    public partial class ConfigureWebhook : Form
    {
        CloudMediaContext _context;
        private INotificationEndPoint _webhookEndpoint;      

        public ConfigWebhookVar WebhookConfig
        {
            get
            {
                return new ConfigWebhookVar()
                {
                    WebhookEndpoint = textBoxEndpointURL.Text,
                    WebhookNotificationJobState = (NotificationJobState)(Enum.Parse(typeof(NotificationJobState), (string)comboBoxJobState.SelectedItem)),
                    WebhookNotificationEndpointType = (NotificationEndPointType)(Enum.Parse(typeof(NotificationEndPointType), (string)comboBoxEndPointType.SelectedItem))

                };
            }

        }



        public ConfigureWebhook(CloudMediaContext myContext, INotificationEndPoint webhookConfig)
        {
            InitializeComponent();
            this.Icon = Bitmaps.Azure_Explorer_ico;

            _context = myContext;
            _webhookEndpoint = webhookConfig;

            PrepareControls();

        }

        private void PrepareControls()
        {
            comboBoxJobState.Items.Clear();           
            comboBoxEndPointType.Items.Clear();

            comboBoxJobState.Items.AddRange(Enum.GetNames(typeof(NotificationJobState)));           
            comboBoxEndPointType.Items.AddRange(Enum.GetNames(typeof(NotificationEndPointType)));
            
            if (_webhookEndpoint == null) // new Webhook Endpoint config
            {
                //WebhookEndpoint
                textBoxEndpointURL.Text = "Http://Endpoint";
                comboBoxJobState.Text = NotificationJobState.FinalStatesOnly.ToString();
                comboBoxEndPointType.Text = NotificationEndPointType.WebHook.ToString();
                checkBoxTaskProgress.Checked = true;

            }
            else // current telemetry config is displayed
            {
                var currentConfig = _context.NotificationEndPoints.Where(n => n.Id == _webhookEndpoint.Id).FirstOrDefault();
                textBoxEndpointURL.Text = currentConfig.EndPointAddress;
                textBoxEndpointURL.Visible = true;
                textBoxEndpointURL.ReadOnly = true;
                buttonDeleteConfig.Visible = true;
                buttonOk.Text = "Update";
                labelWebhookUI.Text = "Current Webhook Settings";
                
               // var settingEndpointType = _webhookEndpoint.EndPointType().Where(s => s.Component == MonitoringComponent.Channel).FirstOrDefault(); ;
                comboBoxEndPointType.Text = _webhookEndpoint.EndPointType.ToString();
                //checkBoxTaskProgress.Checked = currentConfig.
                //comboBoxJobState.Text = currentConfig.GetType
                //var settingSE = _monitorconfig.Settings.ToList().Where(s => s.Component == MonitoringComponent.StreamingEndpoint).FirstOrDefault();
                //if (settingSE != null) comboBoxTaskProgress.Text = settingSE.Level.ToString();
            }
        }


        private void ConfigureWebhook_Load(object sender, EventArgs e)
        {
            moreinfoNotificationEndPointlink.Links.Add(new LinkLabel.Link(0, moreinfoNotificationEndPointlink.Text.Length, Constants.LinkMoreInfoNotificationEndPoint));

        }

        private void moreinfoNotificationEndPointlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }
    }

}
