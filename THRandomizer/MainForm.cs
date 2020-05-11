using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using THRandomizer.FileFormats;

namespace THRandomizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "ECL Script Files|*.ecl";
            open.CheckFileExists = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = open.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "ECL Script Files|*.ecl";
            if (save.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            richTextBox1.ResetText();

            richTextBox1.AppendText("Beginning randomization");

            Random rng;

            int _seed;
            if (seed.Text != string.Empty)
            {
                if (int.TryParse(seed.Text, out int o))
                {
                    _seed = o;
                }
                else
                {
                    int f = int.MinValue;
                    byte[] str = Encoding.UTF8.GetBytes(seed.Text);
                    for (int i = 0; i < str.Length; i++)
                    {
                        f += str[i];
                    }
                    _seed = f;
                }
            }
            else
            {
                _seed = new Random().Next(int.MinValue, int.MaxValue);
            }


            richTextBox1.AppendText($"\nSeed: {_seed}");

            richTextBox1.AppendText("\nReading ECL");
            ECL ecl = new ECL(textBox1.Text);
            richTextBox1.AppendText("\nDone");

            byte[] inFile = File.ReadAllBytes(textBox1.Text);
            MemoryStream stream = new MemoryStream(inFile, 0, inFile.Length, true, true);

            using (BinaryWriter writer = new BinaryWriter(stream))
            {

                if (rngBullets.Checked)
                {
                    rng = new Random(_seed);

                    richTextBox1.AppendText("\nRandomizing bullet data...");

                    for (int i = 0; i < ecl.BulletOffsets.Count; i++)
                    {
                        writer.BaseStream.Seek((int)ecl.BulletOffsets[i] + 0x10, SeekOrigin.Begin);
                        
                        ushort inst = BitConverter.ToUInt16(inFile, (int)ecl.BulletOffsets[i] + 0x4);
                        bool paramMask = false;
                        //Toggle for respecting variables in instructions
                        if (!ignoreVar.Checked) paramMask = Convert.ToBoolean(BitConverter.ToUInt16(inFile, (int)ecl.BulletOffsets[i] + 0x8));

                        switch (inst)
                        {
                            case 602:
                                {
                                    int spr = rng.Next(0, 32);
                                    int color = 0;
                                    if ((spr >= 0 && spr <= 16) || spr == 29 || spr == 31)
                                    {
                                        color = rng.Next(0, 16);
                                    }
                                    else if ((spr >= 18 && spr <= 23) || spr == 26)
                                    {
                                        color = rng.Next(0, 9);
                                    }
                                    else if (spr >= 27 && spr <= 28)
                                    {
                                        color = rng.Next(0,4);
                                    }

                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    writer.Write(spr);
                                    writer.Write(color);
                                    break;
                                }
                            case 603:
                            case 605:
                            case 628:
                            case 704:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 606:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    writer.Write(rng.Next(0, (int)bulletMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)bulletMax.Value + 1));
                                    break;
                                }
                            case 607:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    writer.Write(rng.Next(0, 9));
                                    break;
                                }
                            case 609:
                            case 610:
                            case 611:
                            case 612:
                                {
                                    //Complex bullet transforms, there's a lot going on here because these instructions are very complicated

                                    int j = 0xC;
                                    if (inst == 611 || inst == 612) j = 0x4;

                                    writer.BaseStream.Seek(j, SeekOrigin.Current);
                                    uint type = BitConverter.ToUInt32(inFile, (int)writer.BaseStream.Position);
                                    
                                    if (ECLRandomSettings.ComplexTransTypes.Contains(type))
                                    {
                                        uint t = ECLRandomSettings.ComplexTransTypes[rng.Next(0, ECLRandomSettings.ComplexTransTypes.Length)];
                                        writer.Write(t);

                                        switch (t)
                                        {
                                            case 0x00000001:
                                                {
                                                    writer.Write(-999999);
                                                    writer.Write(-999999);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        writer.Write(-999999f);
                                                        writer.Write(-999999f);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        writer.Write(-999999f);
                                                    }
                                                    break;
                                                }
                                            case 0x00000002:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(-999999);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        writer.Write(-999999f);
                                                        writer.Write(-999999f);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        writer.Write(-999999f);
                                                    }
                                                    break;
                                                }
                                            case 0x00000004:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(-999999);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    break;
                                                }
                                            case 0x00000008:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    break;
                                                }
                                            case 0x00000010:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    break;
                                                }
                                            case 0x00000020:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    break;
                                                }
                                            case 0x00000040:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    break;
                                                }
                                            case 0x00000100:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));

                                                    int w = 0x00;

                                                    w = w | (rng.Next(0, 2) << 0);
                                                    w = w | (rng.Next(0, 2) << 1);
                                                    w = w | (rng.Next(0, 2) << 2);
                                                    w = w | (rng.Next(0, 2) << 3);

                                                    writer.Write(w);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        writer.Write(-999999f);
                                                        writer.Write(-999999f);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        writer.Write(-999999f);
                                                    }
                                                    break;
                                                }
                                            case 0x00000200:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(-999999);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        writer.Write(-999999f);
                                                        writer.Write(-999999f);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        writer.Write(-999999f);
                                                    }
                                                    break;
                                                }
                                            case 0x00000400:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(-999999);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        writer.Write(-999999f);
                                                        writer.Write(-999999f);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        writer.Write(-999999f);
                                                    }
                                                    break;
                                                }
                                            case 0x00000800:
                                                {
                                                    int spr = rng.Next(0, 32);
                                                    int color = 0;
                                                    if ((spr >= 0 && spr <= 16) || spr == 29 || spr == 31)
                                                    {
                                                        color = rng.Next(0, 16);
                                                    }
                                                    else if ((spr >= 18 && spr <= 23) || spr == 26)
                                                    {
                                                        color = rng.Next(0, 9);
                                                    }
                                                    else if (spr >= 27 && spr <= 28)
                                                    {
                                                        color = rng.Next(0, 4);
                                                    }

                                                    writer.Write(spr);
                                                    writer.Write(color);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        writer.Write(-999999f);
                                                        writer.Write(-999999f);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        writer.Write(-999999f);
                                                    }
                                                    break;
                                                }
                                            case 0x00001000:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(-999999);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        writer.Write(-999999f);
                                                        writer.Write(-999999f);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        writer.Write(-999999f);
                                                    }
                                                    break;
                                                }
                                            case 0x20000000:
                                                {
                                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                                    writer.Write(-999999);
                                                    if (inst == 609 || inst == 611)
                                                    {
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    else
                                                    {
                                                        writer.Write(-999999);
                                                        writer.Write(-999999);
                                                        WriteRandomFloat(writer, rng, inFile, paramMask);
                                                    }
                                                    break;
                                                }
                                            default:
                                                break;
                                        }
                                    }

                                    break;
                                }
                            case 624:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 625:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    writer.Write(rng.Next(0, 11));
                                    writer.Write(rng.Next(0, 11));
                                    writer.Write(rng.Next(0, 11));
                                    writer.Write(rng.Next(0, 11));
                                    writer.Write(rng.Next(0, 11));
                                    writer.Write(rng.Next(0, 11));
                                    writer.Write(rng.Next(0, 11));
                                    writer.Write(rng.Next(0, 11));
                                    break;
                                }
                            case 604:
                            case 626:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 627:
                            case 707:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 700:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 701:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            case 705:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 708:
                            case 709:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    break;
                                }
                            default:
                                break;
                        }

                    }
                }
                if (rngEntity.Checked)
                {
                    rng = new Random(_seed);

                    richTextBox1.AppendText("\nRandomizing entity data...");
                    
                    //Need to figure out why this generates things that instantly kill the player or make the player unable to shoot

                    for (int i = 0; i < ecl.EntityOffsets.Count; i++)
                    {
                        writer.BaseStream.Seek((int)ecl.EntityOffsets[i] + 0x10, SeekOrigin.Begin);

                        ushort inst = BitConverter.ToUInt16(inFile, (int)ecl.EntityOffsets[i] + 0x4);
                        bool paramMask = false;
                        //Toggle for respecting variables in instructions
                        if (!ignoreVar.Checked) paramMask = Convert.ToBoolean(BitConverter.ToUInt16(inFile, (int)ecl.EntityOffsets[i] + 0x8));
                        
                        switch (inst)
                        {
                            case 300:
                            case 301:
                            case 304:
                            case 305:
                            case 309:
                            case 310:
                            case 311:
                            case 312:
                                {
                                    uint strLen = BitConverter.ToUInt32(inFile, (int)writer.BaseStream.Position);
                                    writer.BaseStream.Seek(strLen + 0x4, SeekOrigin.Current);

                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            case 306:
                                {
                                    writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                                    writer.Write(rng.Next(0, 5));
                                    break;
                                }
                            case 400:
                            case 402:
                                {
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 401:
                            case 403:
                            case 405:
                            case 436:
                            case 437:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(ECLRandomSettings.AnimationTypes[rng.Next(0, ECLRandomSettings.AnimationTypes.Length)]);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 404:
                            case 406:
                            case 428:
                                {
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 407:
                            case 429:
                            case 431:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(ECLRandomSettings.AnimationTypes[rng.Next(0, ECLRandomSettings.AnimationTypes.Length)]);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 408:
                            case 410:
                                {
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            case 409:
                            case 411:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(ECLRandomSettings.AnimationTypes[rng.Next(0, ECLRandomSettings.AnimationTypes.Length)]);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 412:
                            case 413:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(ECLRandomSettings.AnimationTypes[rng.Next(0, ECLRandomSettings.AnimationTypes.Length)]);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 420:
                            case 422:
                                {
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 421:
                            case 423:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(ECLRandomSettings.AnimationTypes[rng.Next(0, ECLRandomSettings.AnimationTypes.Length)]);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomAngle(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 425:
                            case 426:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 430:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            case 502:
                            case 503:
                                {
                                    writer.Write(ECLRandomSettings.EntityFlags[rng.Next(0, ECLRandomSettings.EntityFlags.Length)]);
                                    break;
                                }
                            case 504:
                                {
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            case 511:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            case 515:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            case 565:
                                {
                                    WriteRandomFloat(writer, rng, inFile, paramMask);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                    }
                }
                if (rngTimers.Checked)
                {
                    rng = new Random(_seed);

                    richTextBox1.AppendText("\nRandomizing timer data...");

                    for (int i = 0; i < ecl.WaitOffsets.Count; i++)
                    {
                        writer.BaseStream.Seek((int)ecl.WaitOffsets[i] + 0x10, SeekOrigin.Begin);

                        ushort inst = BitConverter.ToUInt16(inFile, (int)ecl.WaitOffsets[i] + 0x4);
                        bool paramMask = false;
                        //Toggle for respecting variables in instructions
                        if (!ignoreVar.Checked) paramMask = Convert.ToBoolean(BitConverter.ToUInt16(inFile, (int)ecl.WaitOffsets[i] + 0x8));

                        switch (inst)
                        {
                            case 23:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            case 548:
                                {
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    writer.Write(rng.Next(0, (int)randMax.Value + 1));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                    }
                }
                if (rngDiffFlags.Checked)
                {
                    rng = new Random(_seed);

                    richTextBox1.AppendText($"\nRandomizing difficulty flags...");

                    for (int i = 0; i < ecl.DifficultyFlagOffsets.Count; i++)
                    {
                        writer.Seek((int)ecl.DifficultyFlagOffsets[i], SeekOrigin.Begin);
                        byte o = 0xF0;

                        o = (byte)(o | (rng.Next(0, 2) << 0)); //Easy
                        o = (byte)(o | (rng.Next(0, 2) << 1)); //Normal
                        o = (byte)(o | (rng.Next(0, 2) << 2)); //Hard
                        o = (byte)(o | (rng.Next(0, 2) << 3)); //Lunatic

                        //Console.WriteLine($"Difficulty Flag: 0x{o.ToString("X2")} - L: {dL}, H: {dH}, N: {dN}, E: {dE} | At: 0x{ecl.DifficultyFlagOffsets[i].ToString("X8")}");

                        writer.Write(o);
                    }
                }
            }
            
            File.WriteAllBytes(save.FileName, stream.GetBuffer());
            richTextBox1.AppendText($"\nWritten randomized ECL to {save.FileName}");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (rngDiffFlags.Checked)
            {
                DialogResult d = MessageBox.Show("WARNING: Enabling this option has the potential to break A LOT of things." +
                                               "\n         ONLY ENABLE THIS IF YOU KNOW WHAT YOU'RE DOING!", this.Text, MessageBoxButtons.OKCancel);
                if (d == DialogResult.Cancel)
                    rngDiffFlags.Checked = false;
            }
        }

        float NextFloat(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, random.Next(0, 7));
            return (float)(mantissa * exponent);
        }

        float NextAngle(Random random)
        {
            double rand = random.NextDouble();
            return (float)(Math.PI * rand);
        }

        bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        bool IsPowerOfTwo(float x)
        {
            return ((int)x & ((int)x - 1)) == 0;
        }

        bool IsVar(float v)
        {
            return (v <= -1 && v >= -10) || (v <= -8000 && v >= -10000) || IsPowerOfTwo(v);
        }

        bool IsVar(int v)
        {
            return (v <= -1 && v >= -10) || (v <= -8000 && v >= -10000) || IsPowerOfTwo(v);
        }

        void WriteRandomFloat(BinaryWriter writer, Random random, byte[] file, bool paramMask)
        {
            float v = BitConverter.ToSingle(file, (int)writer.BaseStream.Position);
            if (paramMask && !IsVar(v))
            {
                writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                return;
            }
            writer.Write(NextFloat(random));
        }

        void WriteRandomAngle(BinaryWriter writer, Random random, byte[] file, bool paramMask)
        {
            float v = BitConverter.ToSingle(file, (int)writer.BaseStream.Position);
            if (paramMask && !IsVar(v))
            {
                writer.BaseStream.Seek(0x4, SeekOrigin.Current);
                return;
            }
            writer.Write(NextAngle(random));
        }
    }
}
