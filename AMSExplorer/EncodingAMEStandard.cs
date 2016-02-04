﻿//----------------------------------------------------------------------------------------------
//    Copyright 2015 Microsoft Corporation
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
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using AMSExplorer;


namespace AMSExplorer
{
    public partial class EncodingAMEStandard : Form
    {
        public string EncodingAMEStdPresetJSONFilesUserFolder;
        public string EncodingAMEStdPresetJSONFilesFolder;

        private SubClipConfiguration _subclipConfig;

        public List<IAsset> SelectedAssets;
        private CloudMediaContext _context;

        private bool bMultiAssetMode = true;
        private const string strEditTimes = "Edit times";
        private const string strStitch = "Stitch";
        private const string strAudiooverlay = "Audio overlay";
        private const string strVisualoverlay = "Visual overlay";
        private bool bVisualOverlay = false; // indicate if visual overlay has been checked or not
        private string overlayFilename = string.Empty; // indicate the name of the file to overlay


        private const string defaultprofile = "H264 Multiple Bitrate 720p";
        bool usereditmode = false;

        public readonly IList<Profile> Profiles = new List<Profile> {
            new Profile() {Prof=@"AAC Good Quality Audio", Desc="Produces a single MP4 file containing only stereo audio encoded at 192 kbps."},
            new Profile() {Prof=@"AAC Audio", Desc="Produces a single MP4 file containing only stereo audio encoded at 128 kbps."},
            new Profile() {Prof=@"H264 Multiple Bitrate 1080p Audio 5.1", Desc="Produces a set of 8 GOP-aligned MP4 files, ranging from 6000 kbps to 400 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 1080p", Desc="Produces a set of 8 GOP-aligned MP4 files, ranging from 6000 kbps to 400 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 16x9 for iOS", Desc="Produces a set of 8 GOP-aligned MP4 files, ranging from 8500 kbps to 200 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 16x9 SD Audio 5.1", Desc="Produces a set of 5 GOP-aligned MP4 files, ranging from 1900 kbps to 400 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 16x9 SD", Desc="Produces a set of 5 GOP-aligned MP4 files, ranging from 1900 kbps to 400 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 4K Audio 5.1", Desc="Produces a set of 12 GOP-aligned MP4 files, ranging from 20000 kbps to 1000 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 4K", Desc="Produces a set of 12 GOP-aligned MP4 files, ranging from 20000 kbps to 1000 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 4x3 for iOS", Desc="Produces a set of 8 GOP-aligned MP4 files, ranging from 8500 kbps to 200 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 4x3 SD Audio 5.1", Desc="Produces a set of 5 GOP-aligned MP4 files, ranging from 1600 kbps to 400 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 4x3 SD", Desc="Produces a set of 5 GOP-aligned MP4 files, ranging from 1600 kbps to 400 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 720p Audio 5.1", Desc="Produces a set of 6 GOP-aligned MP4 files, ranging from 3400 kbps to 400 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Multiple Bitrate 720p", Desc="Produces a set of 6 GOP-aligned MP4 files, ranging from 3400 kbps to 400 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Single Bitrate 1080p Audio 5.1", Desc="Produces a single MP4 file with a bitrate of 6750 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Single Bitrate 1080p", Desc="Produces a single MP4 file with a bitrate of 6750 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Single Bitrate 4K Audio 5.1", Desc="Produces a single MP4 file with a bitrate of 18000 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Single Bitrate 4K", Desc="Produces a single MP4 file with a bitrate of 18000 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Single Bitrate 4x3 SD Audio 5.1", Desc="Produces a single MP4 file with a bitrate of 18000 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Single Bitrate 4x3 SD", Desc="Produces a single MP4 file with a bitrate of 18000 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Single Bitrate 16x9 SD Audio 5.1", Desc="Produces a single MP4 file with a bitrate of 2200 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Single Bitrate 16x9 SD", Desc="Produces a single MP4 file with a bitrate of 2200 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Single Bitrate 720p Audio 5.1", Desc="Produces a single MP4 file with a bitrate of 4500 kbps, and AAC 5.1 audio."},
            new Profile() {Prof=@"H264 Single Bitrate 720p for Android", Desc="Produces a single MP4 file with a bitrate of 2000 kbps, and stereo AAC."},
            new Profile() {Prof=@"H264 Single Bitrate 720p", Desc="Produces a single MP4 file with a bitrate of 4500 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Single Bitrate High Quality SD for Android", Desc="Produces a single MP4 file with a bitrate of 500 kbps, and stereo AAC audio."},
            new Profile() {Prof=@"H264 Single Bitrate Low Quality SD for Android", Desc="Produces a single MP4 file with a bitrate of 56 kbps, and stereo AAC audio."}
           };

        private int _nbInputAssets;
        private string _processorVersion;
        private bool _ThumbnailsModeOnly;

        public string EncodingLabel
        {
            set
            {
                labelsummaryjob.Text = value;
            }
        }

        public string EncodingJobName
        {
            get
            {
                return textBoxJobName.Text;
            }
            set
            {
                textBoxJobName.Text = value;
            }
        }

        public string EncodingOutputAssetName
        {
            get
            {
                return textboxoutputassetname.Text;
            }
            set
            {
                textboxoutputassetname.Text = value;
            }
        }


        public string EncodingConfiguration
        {
            get
            {
                return textBoxConfiguration.Text;
            }
        }

        public JobOptionsVar JobOptions
        {
            get
            {
                return buttonJobOptions.GetSettings();
            }
            set
            {
                buttonJobOptions.SetSettings(value);
            }
        }


        public EncodingAMEStandard(CloudMediaContext context, int nbInputAssets, string processorVersion, SubClipConfiguration subclipConfig = null, bool ThumbnailsModeOnly = false)
        {
            InitializeComponent();
            this.Icon = Bitmaps.Azure_Explorer_ico;
            _context = context;
            _processorVersion = processorVersion;
            _subclipConfig = subclipConfig; // used for trimming
            buttonJobOptions.Initialize(_context);
            _nbInputAssets = nbInputAssets;
            _ThumbnailsModeOnly = ThumbnailsModeOnly; // used for thumbnail only mode from the menu
        }


        private void EncodingAMEStandard_Shown(object sender, EventArgs e)
        {
            BuildAssetsPanel();
        }

        private void BuildAssetsPanel()
        {
            tableLayoutPanelIAssets.Visible = false;
            tableLayoutPanelIAssets.ColumnCount += 3;
            foreach (ColumnStyle style in tableLayoutPanelIAssets.ColumnStyles)
            {
                style.SizeType = SizeType.Absolute;
                style.Width = 80;
            }
            tableLayoutPanelIAssets.ColumnStyles[0].SizeType = SizeType.Absolute;
            tableLayoutPanelIAssets.ColumnStyles[0].Width = 20;
            tableLayoutPanelIAssets.ColumnStyles[1].SizeType = SizeType.Absolute;
            tableLayoutPanelIAssets.ColumnStyles[1].Width = 20;
            tableLayoutPanelIAssets.ColumnStyles[2].SizeType = SizeType.Absolute;
            tableLayoutPanelIAssets.ColumnStyles[2].Width = 20;
            tableLayoutPanelIAssets.ColumnStyles[3].SizeType = SizeType.Percent;
            tableLayoutPanelIAssets.ColumnStyles[3].Width = 10;
            tableLayoutPanelIAssets.ColumnStyles[4].SizeType = SizeType.Absolute;
            tableLayoutPanelIAssets.ColumnStyles[4].Width = 100;
            tableLayoutPanelIAssets.ColumnStyles[5].SizeType = SizeType.Absolute;
            tableLayoutPanelIAssets.ColumnStyles[5].Width = 100;
            int i = 0;

            if (SelectedAssets.Count > 1) // Multi assets mode
            {
                bMultiAssetMode = true;
                tableLayoutPanelIAssets.RowCount = SelectedAssets.Count;

                foreach (IAsset asset in SelectedAssets)
                {
                    AddRowControls(i, asset.Name);
                    i++;
                }
                tableLayoutPanelIAssets.Refresh();
            }
            else // Mono asset mode
            {
                bMultiAssetMode = false;
                tableLayoutPanelIAssets.RowCount = SelectedAssets.FirstOrDefault().AssetFiles.Count();
                tabControl1.TabPages[0].Text = "Input files";

                foreach (IAssetFile assetfile in SelectedAssets.FirstOrDefault().AssetFiles)
                {
                    AddRowControls(i, assetfile.Name);
                    i++;
                }
            }
            tableLayoutPanelIAssets.Refresh();
            tableLayoutPanelIAssets.Visible = true;
        }

        private void AddRowControls(int i, string itemName)
        {
            Button butUp = new Button() { Text = char.ConvertFromUtf32(8593), Tag = i, Width = 20 };
            butUp.Click += new System.EventHandler(butUp_Clicked);

            Button butDwn = new Button() { Text = char.ConvertFromUtf32(8595), Tag = i, Width = 20 };
            butDwn.Click += new System.EventHandler(butDwn_Clicked);

            Label Index = new Label()
            {
                Tag = i,
                AutoSize = true,
                Text = i.ToString()
            };

            Label label = new Label()
            {
                Tag = i,
                AutoSize = true,
                Text = itemName
            };

            tableLayoutPanelIAssets.Controls.Add(butUp, 0 /* Column Index */, i /* Row index */);
            tableLayoutPanelIAssets.Controls.Add(butDwn, 1 /* Column Index */, i /* Row index */);
            tableLayoutPanelIAssets.Controls.Add(Index, 2 /* Column Index */, i /* Row index */);
            tableLayoutPanelIAssets.Controls.Add(label, 3 /* Column Index */, i /* Row index */);


            if (IsOverlayFile(itemName))
            {
                CheckBox checkboxVisualOverlay = new CheckBox()
                {
                    Text = strVisualoverlay,
                    Tag = i
                };
                checkboxVisualOverlay.CheckedChanged += new System.EventHandler(checkboxVisualOverlay_CheckedChanged);
                tableLayoutPanelIAssets.Controls.Add(checkboxVisualOverlay, 4 /* Column Index */, i /* Row index */);

            }



            //   tableLayoutPanelIAssets.Controls.Add(checkboxAudioOverlay, 5 /* Column Index */, i /* Row index */);
            //    tableLayoutPanelIAssets.Controls.Add(checkboxStitch, 6 /* Column Index */, i /* Row index */);
            //    tableLayoutPanelIAssets.Controls.Add(checkboxTime, 7 /* Column Index */, i /* Row index */);
            //    tableLayoutPanelIAssets.Controls.Add(textbaseStart, 8 /* Column Index */, i /* Row index */);
            //    tableLayoutPanelIAssets.Controls.Add(textbaseEnd, 9 /* Column Index */, i /* Row index */);

        }

        private bool IsOverlayFile(string filename)
        {
            var mediaFileExtensions = new[] { ".PNG", ".JPG", ".GIF", ".BMP" };
            return (mediaFileExtensions.Contains(Path.GetExtension(filename).ToUpperInvariant()));
        }


        private void checkboxVisualOverlay_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender; // get the checkbox object
            bVisualOverlay = cb.Checked;
            if (bVisualOverlay)
            {
                var position = tableLayoutPanelIAssets.GetPositionFromControl(cb);
                overlayFilename = ((Label)tableLayoutPanelIAssets.GetControlFromPosition(3, position.Row)).Text;
            }
            UpdateCheckboxOverlay(cb);
        }


        private void UpdateCheckboxOverlay(CheckBox cb)
        {
            var position = tableLayoutPanelIAssets.GetPositionFromControl(cb);
            for (int i = 0; i < tableLayoutPanelIAssets.RowCount; i++)
            {
                Control c = tableLayoutPanelIAssets.GetControlFromPosition(position.Column, i); // let find all control from the same row and disable other checkboxes
                if (c != null && i != position.Row && ((!bMultiAssetMode) || (bMultiAssetMode && i != 0)))
                {
                    c.Enabled = !cb.Checked;
                }
            }
            //UpdateStitchAndOverlaysInDoc();
        }

        private void butDwn_Clicked(object sender, EventArgs e)
        {
            var position = tableLayoutPanelIAssets.GetPositionFromControl((Control)sender);
            SwapControls(position.Row, position.Row + 1);
            UpdateControls();
            //   UpdateStitchAndOverlaysInDoc();
        }

        private void butUp_Clicked(object sender, EventArgs e)
        {
            var position = tableLayoutPanelIAssets.GetPositionFromControl((Control)sender);
            SwapControls(position.Row, position.Row - 1);
            UpdateControls();
            //  UpdateStitchAndOverlaysInDoc();
        }

        private void UpdateControls()
        {
            tableLayoutPanelIAssets.GetControlFromPosition(0, 0).Enabled = false; // not possible to go up for first row
            if (bMultiAssetMode)
            {
                CheckBox CBV = ((CheckBox)tableLayoutPanelIAssets.GetControlFromPosition(4, 0));

                if (CBV.Checked)
                {
                    bVisualOverlay = false; // fist row is enabled for visualoverlay now, not possible, so we disable it
                    CBV.Checked = false; // not possible to do overlay with first asset
                }

                CBV.Enabled = false; // not possible to do overlay with first asset
            }

            if (tableLayoutPanelIAssets.RowCount > 1)
            {
                for (int i = 1; i < tableLayoutPanelIAssets.RowCount; i++)
                {
                    tableLayoutPanelIAssets.GetControlFromPosition(0, i).Enabled = true; // button up
                    tableLayoutPanelIAssets.GetControlFromPosition(1, i).Enabled = true; // button down
                    if (!bVisualOverlay) // no visual overlay
                    {
                        tableLayoutPanelIAssets.GetControlFromPosition(4, i).Enabled = true; // checkbox overlay
                    }
                    else // one visual overlay
                    {
                        tableLayoutPanelIAssets.GetControlFromPosition(4, i).Enabled = ((CheckBox)tableLayoutPanelIAssets.GetControlFromPosition(4, i)).Checked;
                    }

                }
            }
            tableLayoutPanelIAssets.GetControlFromPosition(1, tableLayoutPanelIAssets.RowCount - 1).Enabled = false; // not possible to go down for last row
        }


        private void SwapControls(int indexrow1, int indexrow2)
        {
            tableLayoutPanelIAssets.Visible = false;
            for (int col = 0; col < tableLayoutPanelIAssets.ColumnCount; col++)
            {
                if (col != 2) // col = 2 it's Visual Index column
                {
                    Control controw1 = tableLayoutPanelIAssets.GetControlFromPosition(col, indexrow1);
                    Control controw2 = tableLayoutPanelIAssets.GetControlFromPosition(col, indexrow2);
                    tableLayoutPanelIAssets.SetRow(controw1, indexrow2);
                    tableLayoutPanelIAssets.SetRow(controw2, indexrow1);
                    if (bMultiAssetMode) // if we have multiple assets as source, then let's exchange the assets and update the tag
                    {
                        SwapSelectedAssets(indexrow1, indexrow2); // SelectedAssets, 
                        controw1.Tag = indexrow2;
                        controw2.Tag = indexrow1;
                    }
                }
            }
            tableLayoutPanelIAssets.Visible = true;
        }


        private void SwapSelectedAssets(int index1, int index2)
        {
            // If nothing needs to be swapped, just return the original collection.
            if (index1 == index2)
                return;

            // Swap the items.
            IAsset temp = SelectedAssets[index1];
            SelectedAssets[index1] = SelectedAssets[index2];
            SelectedAssets[index2] = temp;
        }

        private void EncodingAMEStandard_Load(object sender, EventArgs e)
        {
            // presets list
            var filePaths = Directory.GetFiles(EncodingAMEStdPresetJSONFilesFolder, "*.json").Select(f => Path.GetFileNameWithoutExtension(f));
            listboxPresets.Items.AddRange(filePaths.ToArray());
            if (!_ThumbnailsModeOnly)
            {
                listboxPresets.SelectedIndex = listboxPresets.Items.IndexOf(defaultprofile);
            }
            else // Thumbnail mode only
            {
                textBoxConfiguration.Text = "{}";
                tabControl1.SelectedTab = tabPageThJPG;
            }
            label4KWarning.Text = string.Empty;
            moreinfoame.Links.Add(new LinkLabel.Link(0, moreinfoame.Text.Length, Constants.LinkMoreInfoMES));
            moreinfopresetslink.Links.Add(new LinkLabel.Link(0, moreinfopresetslink.Text.Length, Constants.LinkMorePresetsMES));
            linkLabelThumbnail1.Links.Add(new LinkLabel.Link(0, linkLabelThumbnail1.Text.Length, Constants.LinkThumbnailsMES));
            linkLabelThumbnail2.Links.Add(new LinkLabel.Link(0, linkLabelThumbnail1.Text.Length, Constants.LinkThumbnailsMES));
            linkLabelThumbnail3.Links.Add(new LinkLabel.Link(0, linkLabelThumbnail1.Text.Length, Constants.LinkThumbnailsMES));

            labelProcessorVersion.Text = string.Format(labelProcessorVersion.Text, _processorVersion);

            if (_subclipConfig != null && _subclipConfig.Trimming)
            {
                timeControlStartTime.SetTimeStamp(_subclipConfig.StartTimeForReencode);
                timeControlEndTime.SetTimeStamp(_subclipConfig.StartTimeForReencode + _subclipConfig.DurationForReencode);
                checkBoxSourceTrimming.Checked = true;
            }
        }


        private void buttonLoadJSON_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.EncodingAMEStdPresetJSONFilesUserFolder))
                openFileDialogPreset.InitialDirectory = this.EncodingAMEStdPresetJSONFilesUserFolder;

            if (openFileDialogPreset.ShowDialog() == DialogResult.OK)
            {
                this.EncodingAMEStdPresetJSONFilesUserFolder = Path.GetDirectoryName(openFileDialogPreset.FileName); // let's save the folder
                try
                {
                    StreamReader streamReader = new StreamReader(openFileDialogPreset.FileName);
                    UpdateTextBoxJSON(streamReader.ReadToEnd());
                    streamReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }

                label4KWarning.Text = string.Empty;
                buttonOk.Enabled = true;
                richTextBoxDesc.Text = string.Empty;

            }
        }


        private void UpdateTextBoxJSON(string jsondata)
        {
            var mode = Program.AnalyseConfigurationString(jsondata);
            if (mode == TypeConfig.XML) // XML data
            {
                textBoxConfiguration.Text = jsondata;
            }
            else if (mode == TypeConfig.JSON) // JSON
            {
                dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsondata);
                if (checkBoxAddAutomatic.Checked)
                {
                    ////////////////////////////
                    // Cleaning of JSON
                    ////////////////////////////

                    // clean auto deinterlaing
                    // clean all sources
                    //if (obj.Sources != null) obj.Sources.Parent.Remove();

                    // clean trimming
                    // clean deinterlace filter
                    if (obj.Sources != null)
                    {
                        var listDelete = new List<dynamic>();
                        foreach (var source in obj.Sources)
                        {
                            if ((source.StartTime != null && source.Duration != null) || (source.Filters != null && source.Filters.Deinterlace != null))
                            {
                                listDelete.Add(source);
                            }
                        }
                        listDelete.ForEach(c => c.Remove());
                        if (obj.Sources.Count == 0)
                        {
                            obj.Sources.Parent.Remove();
                        }
                    }

                    // Clean Insert silent audio track
                    if (obj.Codecs != null)
                    {
                        foreach (var codec in obj.Codecs)
                        {
                            if (codec.Type != null && codec.Type == "AACAudio" && codec.Condition != null && codec.Condition == "InsertSilenceIfNoAudio")
                            {
                                codec.Condition.Parent.Remove();
                            }
                        }
                    }

                    if (obj.Codecs != null) // clean thumbnail entry in Codecs
                    {
                        var listDelete = new List<dynamic>();
                        foreach (var codec in obj.Codecs)
                        {
                            if (codec.JpgLayers != null || codec.PngLayers != null || codec.BmpLayers != null)
                            {
                                listDelete.Add(codec);
                            }
                        }
                        listDelete.ForEach(c => c.Remove());
                    }
                    if (obj.Outputs != null) // clean thumbnail entry in Outputs
                    {
                        var listDelete = new List<dynamic>();
                        foreach (var output in obj.Outputs)
                        {
                            if (output.Format != null && output.Format.Type != null && output.Format.Type.Type == JTokenType.String)
                            {
                                string valuestr = (string)output.Format.Type;
                                if (valuestr == "JpgFormat" || valuestr == "PngFormat" || valuestr == "BmpFormat")
                                {
                                    listDelete.Add(output);
                                }
                            }
                        }
                        listDelete.ForEach(c => c.Remove());
                    }
                    ////////////////////////////
                    // End of Cleaning
                    ////////////////////////////


                    // Trimming
                    if (checkBoxSourceTrimming.Checked)
                    {
                        if (obj.Sources == null)
                        {
                            obj.Sources = new JArray() as dynamic;
                        }

                        dynamic time = new JObject();
                        time.StartTime = timeControlStartTime.GetTimeStampAsTimeSpanWithOffset();
                        time.Duration = timeControlEndTime.GetTimeStampAsTimeSpanWithOffset() - timeControlStartTime.GetTimeStampAsTimeSpanWithOffset();
                        obj.Sources.Add(time);
                    }

                    // Overlay
                    if (false)//bVisualOverlay)
                    {
                        if (obj.Sources == null)
                        {
                            obj.Sources = new JArray() as dynamic;
                        }

                        dynamic Streams = new JArray() as dynamic;
                        obj.Sources.Add(Streams);

                        dynamic Filters = new JObject();
                        dynamic VideoOverlay = new JObject();
                        dynamic Position = new JObject();
                        Position.X = numericUpDownVOverlayRectX;
                        Position.Y = numericUpDownVOverlayRectY;
                        Position.Width = numericUpDownVOverlayRectW;
                        Position.Height = numericUpDownVOverlayRectH;
                        VideoOverlay.Add(Position);
                        VideoOverlay.AudioGainLevel = 0;

                        dynamic MediaParams = new JArray() as dynamic;
                      
                    }

                    // Insert silent audio track
                    if (checkBoxInsertSilentAudioTrack.Checked)
                    {
                        if (obj.Codecs != null)
                        {
                            foreach (var codec in obj.Codecs)
                            {
                                if (codec.Type != null && codec.Type == "AACAudio")
                                {
                                    codec.Condition = "InsertSilenceIfNoAudio";
                                }
                            }
                        }
                    }

                    // Insert disable auto deinterlacing
                    if (checkBoxDisableAutoDeinterlacing.Checked)
                    {
                        if (obj.Sources == null)
                        {
                            obj.Sources = new JArray() as dynamic;
                        }

                        bool DeinterModeSet = false;
                        foreach (var source in obj.Sources)
                        {
                            if (source.Filters != null)
                            {
                                if (source.Filters.Deinterlace != null)
                                {
                                    source.Filters.Deinterlace.Mode = "Off";
                                }
                                else
                                {
                                    dynamic modeeentry = new JObject() as dynamic;
                                    modeeentry.Mode = "Off";
                                    source.Filters.Deinterlace = modeeentry;
                                }
                                DeinterModeSet = true;
                            }
                        }

                        if (!DeinterModeSet)
                        {
                            dynamic sourceentry = new JObject() as dynamic;
                            dynamic deinterlaceentry = new JObject() as dynamic;
                            dynamic modeeentry = new JObject() as dynamic;
                            modeeentry.Mode = "Off";
                            deinterlaceentry.Deinterlace = modeeentry;
                            sourceentry.Filters = deinterlaceentry;
                            obj.Sources.Add(sourceentry);
                        }


                    }


                    if (_subclipConfig != null) // subclipping. we need to add top bitrate values
                    {
                        if (obj.Sources == null)
                        {
                            obj.Sources = new JArray() as dynamic;
                        }

                        dynamic entry = new JObject() as dynamic;

                        bool alreadyentry = false;
                        if (obj.Sources.Count > 0)
                        {
                            entry = obj.Sources[0];
                            alreadyentry = true;
                        }

                        entry.Streams = new JArray() as dynamic;

                        dynamic stream = new JObject();
                        stream.Type = "AudioStream";
                        stream.Value = "TopBitrate";
                        entry.Streams.Add(stream);

                        stream = new JObject();
                        stream.Type = "VideoStream";
                        stream.Value = "TopBitrate";
                        entry.Streams.Add(stream);

                        if (!alreadyentry) obj.Sources.Add(entry);
                    }


                    // Thumbnails settings
                    if (checkBoxGenThumbnailsJPG.Checked || checkBoxGenThumbnailsPNG.Checked || checkBoxGenThumbnailsBMP.Checked)
                    {
                        if (obj.Codecs == null)
                        {
                            obj.Codecs = new JArray() as dynamic;
                        }


                        if (obj.Outputs == null)
                        {
                            obj.Outputs = new JArray() as dynamic;
                        }

                        if (checkBoxGenThumbnailsJPG.Checked)
                        {
                            dynamic thOutputEntry = new JObject();
                            thOutputEntry.FileName = textBoxThFileNameJPG.Text;
                            dynamic Format = new JObject();

                            dynamic thEntry = new JObject();
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeStartJPG.Text)) thEntry.Start = textBoxThTimeStartJPG.Text;
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeStepJPG.Text)) thEntry.Step = textBoxThTimeStepJPG.Text;
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeRangeJPG.Text)) thEntry.Range = textBoxThTimeRangeJPG.Text;

                            thEntry.Type = "JpgImage";
                            thEntry.JpgLayers = new JArray() as dynamic;
                            dynamic JpgLayer = new JObject();
                            JpgLayer.Quality = (int)numericUpDownThQuality.Value;
                            JpgLayer.Type = "JpgLayer";
                            JpgLayer.Width = (int)numericUpDownThWidthJPG.Value;
                            JpgLayer.Height = (int)numericUpDownThHeightJPG.Value;
                            thEntry.JpgLayers.Add(JpgLayer);
                            obj.Codecs.Add(thEntry);

                            Format.Type = "JpgFormat";
                            thOutputEntry.Format = Format;
                            obj.Outputs.Add(thOutputEntry);
                        }
                        if (checkBoxGenThumbnailsPNG.Checked)
                        {
                            dynamic thOutputEntry = new JObject();
                            thOutputEntry.FileName = textBoxThFileNamePNG.Text;
                            dynamic Format = new JObject();

                            dynamic thEntry = new JObject();
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeStartPNG.Text)) thEntry.Start = textBoxThTimeStartPNG.Text;
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeStepPNG.Text)) thEntry.Step = textBoxThTimeStepPNG.Text;
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeRangePNG.Text)) thEntry.Range = textBoxThTimeRangePNG.Text;

                            thEntry.Type = "PngImage";
                            thEntry.PngLayers = new JArray() as dynamic;
                            dynamic PngLayer = new JObject();
                            PngLayer.Type = "PngLayer";
                            PngLayer.Width = (int)numericUpDownThWidthPNG.Value;
                            PngLayer.Height = (int)numericUpDownThHeightPNG.Value;
                            thEntry.PngLayers.Add(PngLayer);
                            obj.Codecs.Add(thEntry);

                            Format.Type = "PngFormat";
                            thOutputEntry.Format = Format;
                            obj.Outputs.Add(thOutputEntry);
                        }
                        if (checkBoxGenThumbnailsBMP.Checked)
                        {
                            dynamic thOutputEntry = new JObject();
                            thOutputEntry.FileName = textBoxThFileNameBMP.Text;
                            dynamic Format = new JObject();

                            dynamic thEntry = new JObject();
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeStartBMP.Text)) thEntry.Start = textBoxThTimeStartBMP.Text;
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeStepBMP.Text)) thEntry.Step = textBoxThTimeStepBMP.Text;
                            if (!string.IsNullOrWhiteSpace(textBoxThTimeRangeBMP.Text)) thEntry.Range = textBoxThTimeRangeBMP.Text;

                            thEntry.Type = "BmpImage";
                            thEntry.BmpLayers = new JArray() as dynamic;
                            dynamic BmpLayer = new JObject();
                            BmpLayer.Type = "BmpLayer";
                            BmpLayer.Width = (int)numericUpDownThWidthBMP.Value;
                            BmpLayer.Height = (int)numericUpDownThHeightBMP.Value;
                            thEntry.BmpLayers.Add(BmpLayer);
                            obj.Codecs.Add(thEntry);

                            Format.Type = "BmpFormat";
                            thOutputEntry.Format = Format;
                            obj.Outputs.Add(thOutputEntry);
                        }
                    }
                }
                textBoxConfiguration.Text = obj.ToString();
            }
            else // no xml and no Json !
            {
                textBoxConfiguration.Text = jsondata;
            }
        }


        private void buttonSaveXML_Click(object sender, EventArgs e)
        {
            if (saveFileDialogPreset.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialogPreset.FileName, textBoxConfiguration.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
                }

            }
        }


        private void listboxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listboxPresets.SelectedItem != null)
            {
                try
                {
                    string filePath = Path.Combine(EncodingAMEStdPresetJSONFilesFolder, listboxPresets.SelectedItem.ToString() + ".json");
                    StreamReader streamReader = new StreamReader(filePath);
                    usereditmode = false;
                    UpdateTextBoxJSON(streamReader.ReadToEnd());
                    usereditmode = true;
                    streamReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk or process the JSON data. Original error:" + Constants.endline + ex.Message);
                    usereditmode = true;
                }

                if (listboxPresets.SelectedItem.ToString().Contains("4K") && _context.EncodingReservedUnits.FirstOrDefault().ReservedUnitType != ReservedUnitType.Premium)
                {
                    label4KWarning.Text = (string)label4KWarning.Tag;
                    buttonOk.Enabled = false;
                }
                else
                {
                    label4KWarning.Text = string.Empty;
                    buttonOk.Enabled = true;
                }

                var profile = Profiles.Where(p => p.Prof == listboxPresets.SelectedItem.ToString()).FirstOrDefault();
                if (profile != null)
                {
                    richTextBoxDesc.Text = profile.Desc;
                }
                else
                {
                    richTextBoxDesc.Text = string.Empty;
                }

            }
        }

        private void textBoxConfiguration_TextChanged(object sender, EventArgs e)
        {
            if (usereditmode)
            {
                listboxPresets.SelectedIndex = -1;
                richTextBoxDesc.Text = string.Empty;
            }

            // let's normalize the line breaks
            textBoxConfiguration.Text = textBoxConfiguration.Text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);

            bool Error = false;
            var type = Program.AnalyseConfigurationString(textBoxConfiguration.Text);
            if (type == TypeConfig.JSON)
            {
                // Let's check JSON syntax

                try
                {
                    var jo = JObject.Parse(textBoxConfiguration.Text);
                }
                catch (Exception ex)
                {
                    labelWarningJSON.Text = string.Format((string)labelWarningJSON.Tag, ex.Message);
                    Error = true;
                }
            }
            else if (type == TypeConfig.XML) // XML 
            {
                try
                {
                    var xml = XElement.Load(new StringReader(textBoxConfiguration.Text));
                }
                catch (Exception ex)
                {
                    labelWarningJSON.Text = string.Format("Error in XML data: {0}", ex.Message);
                    Error = true;
                }
            }

            labelWarningJSON.Visible = Error;
        }

        private void moreinfoame_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void moreinfopresetslink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }


        private void timeControlStartTime_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBoxJSON(textBoxConfiguration.Text);
            UpdateDurationText();
        }

        private void timeControlDuration_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBoxJSON(textBoxConfiguration.Text);
            UpdateDurationText();
        }
        private void UpdateDurationText()
        {
            textBoxSourceDurationTime.Text = (timeControlEndTime.GetTimeStampAsTimeSpanWithOffset() - timeControlStartTime.GetTimeStampAsTimeSpanWithOffset()).ToString();
        }

        private void checkBoxSourceTrimming_CheckedChanged(object sender, EventArgs e)
        {
            timeControlStartTime.Enabled =
                timeControlEndTime.Enabled =
                textBoxSourceDurationTime.Enabled =
                checkBoxSourceTrimming.Checked;

            UpdateTextBoxJSON(textBoxConfiguration.Text);
        }

        private void checkBoxAddAutomatic_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void checkBoxGenThumbnails_CheckedChanged(object sender, EventArgs e)
        {
            panelThumbnailsJPG.Enabled = checkBoxGenThumbnailsJPG.Checked;
            UpdateTextBoxJSON(textBoxConfiguration.Text);
        }

        private void ThumbnailSettingsChanged(object sender, EventArgs e)
        {
            UpdateTextBoxJSON(textBoxConfiguration.Text);
        }

        private void checkBoxGenThumbnailsPNG_CheckedChanged(object sender, EventArgs e)
        {
            panelThumbnailsPNG.Enabled = checkBoxGenThumbnailsPNG.Checked;
            UpdateTextBoxJSON(textBoxConfiguration.Text);
        }

        private void checkBoxGenThumbnailsBMP_CheckedChanged(object sender, EventArgs e)
        {
            panelThumbnailsBMP.Enabled = checkBoxGenThumbnailsBMP.Checked;
            UpdateTextBoxJSON(textBoxConfiguration.Text);
        }

        private void linkLabelThumbnail1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void checkBoxInsertSilentAudioTrack_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTextBoxJSON(textBoxConfiguration.Text);
        }

        private void checkBoxDisableAutoDeinterlacing_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTextBoxJSON(textBoxConfiguration.Text);
        }
    }

    enum TypeConfig
    {
        JSON,
        XML,
        Other
    }


}
