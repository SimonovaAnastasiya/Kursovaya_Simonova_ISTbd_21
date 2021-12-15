using CourseWork.Model;
using System.Windows.Controls;
using CourseWork.View.UserControls;
using CourseWork.Model.Data.Service;
using System.Text.RegularExpressions;
using System.Windows;

namespace CourseWork.ViewModel
{
    public class ReservationCanvasVM : BaseVM
    {
        #region PROPERTIES


        public static string UserIconPath { get => "/Resources/img/UserIcon.png"; }

        private readonly string _nickname = AuthVM.Nickname;
        public string UserNickname { get => _nickname ?? "Default user"; }

        private UserControl currentUserControl = new ReservationGridContentUC();

        public UserControl CurrentUserControl
        {
            get { return currentUserControl; }
            set { currentUserControl = value; NotifyPropertyChanged(nameof(CurrentUserControl)); }
        }

        private UserControl prevUserControl;

        public UserControl PrevUserControl
        {
            get { return prevUserControl; }
            set { prevUserControl = value; NotifyPropertyChanged(nameof(PrevUserControl)); }
        }
        private string userSearchNickname;

        public string UserSearchNickname
        {
            get { return userSearchNickname; }
            set { userSearchNickname = value; NotifyPropertyChanged(nameof(UserSearchNickname)); }
        }



        private bool isEnabledPrevBtn = true;

        public bool IsEnabledPrevBtn
        {
            get { return isEnabledPrevBtn; }
            set { isEnabledPrevBtn = value; NotifyPropertyChanged(nameof(IsEnabledPrevBtn)); }
        }
        private bool isEnabledNextBtn = true;

        public bool IsEnabledNextBtn
        {
            get { return isEnabledNextBtn; }
            set { isEnabledNextBtn = value; NotifyPropertyChanged(nameof(IsEnabledNextBtn)); }
        }

        #endregion
        #region COMMANDS

        private RelayCommand userControlLoaded;


        private RelayCommand nextBtnClick;
        public RelayCommand NextBtnClick { get => nextBtnClick ?? new(o => _NextBtnClick()); }

        private RelayCommand prevBtnClick;
        public RelayCommand PrevBtnClick { get => prevBtnClick ?? new(o => _PrevBtnClick()); }

        private RelayCommand checkUserByLogin;

        public RelayCommand CheckUserByLogin { get => checkUserByLogin ?? new(o => _CheckUserByLogin()); }

        #endregion
        #region METHODS
        private void _NextBtnClick()
        {
            switch (CurrentUserControl)
            {
                case ReservationGridContentUC:
                    PrevUserControl = CurrentUserControl;
                    CurrentUserControl = new ReservationGridContentNextUC();
                    IsEnabledNextBtn = false;
                    break;
                case ReservationGridContentPastUC:
                    PrevUserControl = CurrentUserControl;
                    CurrentUserControl = new ReservationGridContentUC();
                    IsEnabledNextBtn = true;
                    IsEnabledPrevBtn = true;
                    break;
            }
        }

        private void _PrevBtnClick()
        {
            switch (CurrentUserControl)
            {
                case ReservationGridContentUC:
                    PrevUserControl = CurrentUserControl;
                    CurrentUserControl = new ReservationGridContentPastUC();
                    IsEnabledPrevBtn = false;
                    break;
                case ReservationGridContentNextUC:
                    PrevUserControl = CurrentUserControl;
                    CurrentUserControl = new ReservationGridContentUC();
                    IsEnabledPrevBtn = true;
                    IsEnabledNextBtn = true;
                    break;
            }
        }
        private void _CheckUserByLogin()
        {
            bool isExist = false;
            foreach (Reservation reservation in ReservationService.GetAllReservations())
                if (Regex.IsMatch(reservation.Members, $@"\b{UserSearchNickname}\b"))
                    isExist = true;
            UserSearchNickname = "";
            MessageBox.Show(isExist
                                    ? "Such a user found in the reserved!"
                                    : "Such a user not found in the reserved!",
                                    "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}