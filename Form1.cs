using Cinema_seat_reservation_system.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema_seat_reservation_system
{
    public partial class Form1 : Form
    {
        List<Button> seatsList = new List<Button>();
        string AllSeats = "";

        Image OriginalImage;
        string OriginalText;

        Color cAvailable = Color.FromArgb(0, 0, 0, 0);
        Color cTaken = Color.FromArgb(0, 1, 0, 0);
        Color cReserved = Color.FromArgb(0, 2, 0, 0);
        Color cBooked = Color.FromArgb(0, 3, 0, 0);

        Image imgReservedSeat = Resources.GreenSeat;
        Image imgTakenSeat = Resources.RedSeat;
        Image imgBlankSeat = Resources.BlankSeat;
        Image imgBookedSeat = Resources.BlueSeat;
        Image imgHoverSeat = Resources.GraySeat;

        int totalTicketsPrice = 0;

        enum enSeatMode { eAvailable = 0,  eTaken = 1, eReserved = 2, eBooked = 3};
        public Form1()
        {
            InitializeComponent();
            DefaultSettings();
        }

        //Settings
        void DefaultSettings()
        {
            SetSomeSeatsAvailable();
            SetSomeSeatsTaken();
            ResetSettings();
        }
        void SetSomeSeatsAvailable()
        {
            MakeSeatAvailable(A2);
            MakeSeatAvailable(A3);
            MakeSeatAvailable(A5V);
            MakeSeatAvailable(A6);
            MakeSeatAvailable(A7);
            MakeSeatAvailable(A8);

            MakeSeatAvailable(B1);
            MakeSeatAvailable(B2);
            MakeSeatAvailable(B5);
            MakeSeatAvailable(B7);
            MakeSeatAvailable(B8);

            MakeSeatAvailable(C3);
            MakeSeatAvailable(C4);
            MakeSeatAvailable(C5);
            MakeSeatAvailable(C6);
            MakeSeatAvailable(C7);
            MakeSeatAvailable(C8);

            MakeSeatAvailable(D1);
            MakeSeatAvailable(D2);
            MakeSeatAvailable(D3);
            MakeSeatAvailable(D4);
            MakeSeatAvailable(D5);
            MakeSeatAvailable(D6);
            MakeSeatAvailable(D8);
        }
        void SetSomeSeatsTaken()
        {
            MakeSeatTaken(A1);
            MakeSeatTaken(A4V);
            MakeSeatTaken(B3);
            MakeSeatTaken(B4);
            MakeSeatTaken(B6);
            MakeSeatTaken(C1);
            MakeSeatTaken(C2);
            MakeSeatTaken(D7);
        }

        void ResetSettings()
        {
            seatsList.Clear();
            totalTicketsPrice = 0;
            UpdateTotalPrice();
            lbSeatsCount.Text = "0";
            lbSeatsReserved.Text = "";
        }

        //Modes
        enSeatMode GetSeatMode(Button seat)
        {
            Color btnColor = seat.BackColor;

            if (btnColor == cAvailable)
                return enSeatMode.eAvailable;
            if (btnColor == cTaken)
                return enSeatMode.eTaken;
            if (btnColor == cReserved)
                return enSeatMode.eReserved;
            if (btnColor == cBooked)
                return enSeatMode.eBooked;

            return enSeatMode.eAvailable;
        }
        void ChangeSeatMode(Button seat, enSeatMode mode)
        {
            switch (mode)
            {
                case enSeatMode.eAvailable:
                    seat.BackColor = cAvailable;
                    seat.BackgroundImage = imgBlankSeat;
                    break;
                case enSeatMode.eTaken:
                    seat.BackColor = cTaken;
                    seat.BackgroundImage = imgTakenSeat;
                    break;
                case enSeatMode.eReserved:
                    seat.BackColor = cReserved;
                    seat.BackgroundImage = imgReservedSeat;
                    break;
                case enSeatMode.eBooked:
                    seat.BackColor = cBooked;
                    seat.BackgroundImage = imgBookedSeat;
                    break;
                default:
                    break;
            }
        }

        void MakeSeatAvailable(Button seat)
        {
            ChangeSeatMode(seat, enSeatMode.eAvailable);
            OriginalImage = imgBlankSeat;
        }
        void MakeSeatReserved(Button seat)
        {
            ChangeSeatMode(seat, enSeatMode.eReserved);
            OriginalImage = imgReservedSeat;
        }
        void MakeSeatTaken(Button seat)
        {
            ChangeSeatMode(seat, enSeatMode.eTaken);
            OriginalImage = imgTakenSeat;
        }
        void MakeSeatBooked(Button seat)
        {
            ChangeSeatMode(seat, enSeatMode.eBooked);
            OriginalImage = imgBookedSeat;
        }


        bool IsAvailableSeat(Button seat)
        {
            if (GetSeatMode(seat) == enSeatMode.eAvailable)
                return true;
            return false;
        }
        bool IsReservedSeat(Button seat)
        {
            if (GetSeatMode(seat) == enSeatMode.eReserved)
                return true;
            return false;
        }

        //Price
        void UpdateTotalPrice()
        {
            lbTotalTicketsPrice.Text = "$" + totalTicketsPrice.ToString();
        }
        //Seats
        void UpdateSeatsList()
        {
            AllSeats = "";

            foreach (Button seat in seatsList)
            {
                AllSeats += seat.Text.ToString() + " ";
            }
            lbSeatsReserved.Text = AllSeats;
            lbSeatsCount.Text = seatsList.Count.ToString();
        }

        void DeleteSeat(Button seat)
        {
            totalTicketsPrice-= Convert.ToInt32(seat.Tag);
            seatsList.Remove(seat);
        }

        void ReserveSeat(Button seat)
        {
            if (IsReservedSeat(seat))
            {
                MakeSeatAvailable(seat);
                DeleteSeat(seat);

                UpdateTotalPrice();
                UpdateSeatsList();
                return;
            }

            if (!IsAvailableSeat(seat))
            {
                MessageBox.Show("This seat is Unavailable.", "Unavailable seat!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            seatsList.Add(seat);
            totalTicketsPrice += Convert.ToInt32(seat.Tag);
            UpdateTotalPrice();
            UpdateSeatsList();  

            MakeSeatReserved(seat);
        }

        //Confirmation
        void ConfirmReservedSeats()
        {
            foreach (Button seat in seatsList)
                MakeSeatBooked(seat);
        }
        void CancelReservedSeats()
        {
            foreach (Button seat in seatsList)
                MakeSeatAvailable(seat);
        }
        bool ConfirmMessage()
        {
            if (seatsList.Count == 0)
            {
                MessageBox.Show("You did not reserve any seats!", "Confirm Reversation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string message = "";
            message += "Are you sure you want to book these seats : " + AllSeats + Environment.NewLine;
            message += "For $" + totalTicketsPrice + " ?";
            if ((MessageBox.Show(message, "Confirm Reversation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)) == DialogResult.OK)
            {
                return true;
            }
            return false;
        }
        void BookSeats()
        {
            if (!ConfirmMessage())
                return;

            ConfirmReservedSeats();
            ResetSettings();
        }

        void Reset()
        {
            CancelReservedSeats();
            ResetSettings();
        }

        //Events
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSeat_MouseEnter(object sender, EventArgs e)
        {
            OriginalImage = ((Button)sender).BackgroundImage;
            OriginalText = ((Button)sender).Text;

            ((Button)sender).Text = "$" + ((Button)sender).Tag;
            ((Button)sender).BackgroundImage = imgHoverSeat;
            ((Button)sender).ForeColor = Color.Lime;
        }
        private void btnSeat_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundImage = OriginalImage;
            ((Button)sender).Text = OriginalText;
            ((Button)sender).ForeColor = Color.Black;
        }


        private void btnSeat_Click(object sender, EventArgs e)
        {
            ((Button)sender).Text = OriginalText;
            ((Button)sender).ForeColor = Color.Black;
            ReserveSeat((Button)sender);
        }

        private void btn_Enter(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Gray;
            ((Button)sender).ForeColor = Color.White;
        }
        private void btnConfirm_MouseLeave(object sender, EventArgs e)
        {
            btnConfirm.BackColor = Color.Lime;
            btnConfirm.ForeColor = Color.Black;
        }
        private void btnReset_MouseLeave(object sender, EventArgs e)
        {
            btnReset.BackColor = Color.Silver;
            btnReset.ForeColor = Color.Black;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            BookSeats();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}