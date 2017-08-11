using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;

namespace TeleBanel
{
    public static class BotKeyboardCollection
    {
        public static KeyboardButton[][] GetCommonReplyKeyboard()
        {
            var commonKeyboard = new[]
            {
                new[]
                {
                    new KeyboardButton(Emoji.Pencil + "Edit Job"),
                    new KeyboardButton("Add Job"),
                    new KeyboardButton("Delete Job")
                },
                new[]
                {
                    new KeyboardButton("Edit Title"),
                    new KeyboardButton("Edit Footer"),
                    new KeyboardButton("Edit Logo")
                },
                new[]
                {
                    new KeyboardButton("Inbox"),
                    new KeyboardButton("Edit Social Links"),
                    new KeyboardButton("Edit About")
                },
                new[]
                {
                    new KeyboardButton("Change Language"),
                    new KeyboardButton("Help"),
                    new KeyboardButton("About")
                }
            };

            return commonKeyboard;
        }
        public static KeyboardButton[] GetRegisterReplyKeyboard()
        {
            var keyboard = new[]
            {
                new KeyboardButton(Localization.Register),
                new KeyboardButton(Localization.GetMyId),
                new KeyboardButton(Localization.ChangeLanguage)
            };

            return keyboard;
        }
        public static InlineKeyboardButton[][] GetVoteInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton("TESTs", "Alert"),
                    new InlineKeyboardCallbackButton("CallbackData", "NoAlert")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardSwitchInlineQueryButton("SwitchInlineQueryCurrentChat"),
                    new InlineKeyboardSwitchInlineQueryCurrentChatButton("SwitchInlineQuery", "/register")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardUrlButton("Url", "https://dezire.pro")
                }
            };

            return inlineKeys;
        }
        public static InlineKeyboardButton[][] GetPassKeyboardInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap7, "Num.7"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap8, "Num.8"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap9, "Num.9")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap4, "Num.4"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap5, "Num.5"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap6, "Num.6")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap1, "Num.1"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap2, "Num.2"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap3, "Num.3")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.LeftArrow, "Backspace"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap0, "Num.0"),
                    new InlineKeyboardCallbackButton(Emoji.RightArrowCurvingLeft, "Enter")
                }
            };

            return inlineKeys;
        }
    }
}