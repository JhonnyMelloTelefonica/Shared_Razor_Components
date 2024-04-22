using Shared_Static_Class.Model_DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components.Web;

namespace Shared_Razor_Components.Shared
{
    public class ViewOptionService : INotifyPropertyChanged
    {
        public IDictionary<string, ACESSOS_MOBILE_CHAT_DTO> data = new Dictionary<string, ACESSOS_MOBILE_CHAT_DTO>();

        public List<ChatOpenService> ChatsOpened { get; set; } = new();

        public async Task OpenNewChat(ChatOpenService service)
        {
            ChatsOpened.Add(service);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatsOpened)));
        }
        public async Task CloseChat(ChatOpenService service)
        {
            ChatsOpened.Remove(service);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatsOpened)));
        }

        public string ActualPlataformUrl = string.Empty;

        private bool _buttonCloseplataform = false;
        public bool buttonCloseplataform
        {
            get => _buttonCloseplataform;
            set
            {
                if (_buttonCloseplataform == value) return;
                _buttonCloseplataform = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(buttonCloseplataform)));
            }
        }
        private bool _chatBarVisible = false;
        public bool ChatBarVisible
        {
            get => _chatBarVisible;
            set
            {
                if (_chatBarVisible == value) return;
                _chatBarVisible = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatBarVisible)));
            }
        }

        private bool _navBarVisible = false;
        public bool NavBarVisible
        {
            get => _navBarVisible;
            set
            {
                if (_navBarVisible == value) return;
                _navBarVisible = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavBarVisible)));
            }
        }
        private bool _navBarPinned = false;
        public bool NavBarPinned
        {
            get => _navBarPinned;
            set
            {
                if (_navBarPinned == value) return;
                _navBarPinned = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavBarPinned)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        //Change state by click on the button

        private Timer timer;

        public async Task HandleMouseEnter(MouseEventArgs args)
        {
            timer = new Timer(_ =>
            {
                ToggleNavMenu();
            }, null, 500, Timeout.Infinite);
        }

        public void HandleMouseLeave()
        {
            timer?.Dispose();
        }

        
        public void ToggleNavMenu()
        {
            if (!NavBarPinned)
            {
                NavBarVisible = !NavBarVisible;//Change
            }
        }

        public void ToggleChatMenu()
        {
            ChatBarVisible = !ChatBarVisible;//Change
        }

        //get additional css class for nav bar div
        public string NavBarClass
        {
            get
            {
                return NavBarVisible ? "flyout-opened" : "flyout-closed";//d-none class will hide the div
            }
        }
        //get additional css class for nav bar div
        public string ChatBarClass
        {
            get
            {
                return ChatBarVisible ? "chatbar-opened" : "chatbar-closed";//d-none class will hide the div
            }
        }

    }
    public class ChatOpenService : INotifyPropertyChanged
    {
        public ACESSOS_MOBILE_CHAT_DTO user { get; set; } = new();
        private bool _chatBarVisible = true;
        public bool ChatBarVisible
        {
            get => _chatBarVisible;
            set
            {
                if (_chatBarVisible == value) return;
                _chatBarVisible = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatBarVisible)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void ToggleChat()
        {
            ChatBarVisible = !ChatBarVisible;//Change
        }

        public string ChatBarClass
        {
            get
            {
                return ChatBarVisible ? "chatbar-opened" : "chatbar-closed";//d-none class will hide the div
            }
        }
    }

}