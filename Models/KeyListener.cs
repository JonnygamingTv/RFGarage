using System.Collections.Generic;
using System.Threading;
using Rocket.Unturned.Player;
using RocketExtensions.Utilities;

namespace RFRocketLibrary.Models
{
    // Original Author: ShimmyMySherbet
    /// <summary>
    /// Tool to watch for player key events. Remember to call Stop() when your finished with it, or it will continue to run.
    /// </summary>
    public class KeyListener
    {
        private readonly UnturnedPlayer _player;

        /// <summary>
        /// How many ms between updates. Lower value means higher accuracy, but too low and it could cause issues.
        /// </summary>
        private const int UpdateRate = 100;

        private bool _isRunning;
        private readonly Dictionary<int, bool> _lastMapping = new();

        public delegate void Key(UnturnedPlayer player, UnturnedKey key);
        public delegate void KeyUpdated(UnturnedPlayer player, UnturnedKey key, bool isPressed);

        public event KeyUpdated? OnKeyUpdated;

        public event Key? OnKeyUp;

        public event Key? OnKeyDown;

        public bool JumpKeyDown => _lastMapping[(int)UnturnedKey.Jump];
        public bool SprintKeyDown => _lastMapping[(int)UnturnedKey.Sprint];
        public bool LPunchKeyDown => _lastMapping[(int)UnturnedKey.LPunch];
        public bool RPunchKeyDown => _lastMapping[(int)UnturnedKey.RPunch];
        public bool ProneKeyDown => _lastMapping[(int)UnturnedKey.Prone];
        public bool CrouchKeyDown => _lastMapping[(int)UnturnedKey.Crouch];
        public bool LeanLeftKeyDown => _lastMapping[(int)UnturnedKey.LeanLeft];
        public bool LeanRightKeyDown => _lastMapping[(int)UnturnedKey.LeanRight];
        public bool CodeHotkey1Down => _lastMapping[(int)UnturnedKey.CodeHotkey1];
        public bool CodeHotkey2Down => _lastMapping[(int)UnturnedKey.CodeHotkey2];
        public bool CodeHotkey3Down => _lastMapping[(int)UnturnedKey.CodeHotkey3];
        public bool CodeHotkey4Down => _lastMapping[(int)UnturnedKey.CodeHotkey4];
        public bool CodeHotkey5Down => _lastMapping[(int)UnturnedKey.CodeHotkey5];

        public KeyListener(UnturnedPlayer player)
        {
            _player = player;
            _isRunning = true;
            var runThread = new Thread(UpdateLoop);
            runThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private async void UpdateLoop()
        {
            while (_isRunning)
            {
                for (var i = 0; i < _player.Player.input.keys.Length - 1; i++)
                {
                    var current = _player.Player.input.keys[i];
                    if (_lastMapping.ContainsKey(i))
                    {
                        var last = _lastMapping[i];
                        if (last == current) 
                            continue;
                        
                        var key = IntToUKey(i);
                        if (key != UnturnedKey.Unknown)
                        {
                            await ThreadTool.RunOnGameThreadAsync(() => { OnKeyUpdated?.Invoke(_player, key, current);});
                            if (current)
                            {
                                await ThreadTool.RunOnGameThreadAsync(() => { OnKeyDown?.Invoke(_player, key);});
                            }
                            else
                            {
                                await ThreadTool.RunOnGameThreadAsync(() => { OnKeyUp?.Invoke(_player, key);});
                            }
                        }
                        
                        _lastMapping[i] = current;
                    }
                    else
                    {
                        _lastMapping[i] = current;
                    }
                }
                Thread.Sleep(UpdateRate);
            }
        }

        private static UnturnedKey IntToUKey(int i)
        {
            return i switch
            {
                0 => UnturnedKey.Jump,
                1 => UnturnedKey.LPunch,
                2 => UnturnedKey.RPunch,
                3 => UnturnedKey.Crouch,
                4 => UnturnedKey.Prone,
                5 => UnturnedKey.Sprint,
                6 => UnturnedKey.LeanLeft,
                7 => UnturnedKey.LeanRight,
                9 => UnturnedKey.CodeHotkey1,
                10 => UnturnedKey.CodeHotkey2,
                11 => UnturnedKey.CodeHotkey3,
                12 => UnturnedKey.CodeHotkey4,
                13 => UnturnedKey.CodeHotkey5,
                _ => UnturnedKey.Unknown
            };
        }
    }

    public enum UnturnedKey
    {
        Unknown = -1,
        Jump = 0,
        LPunch = 1,
        RPunch = 2,
        /// <summary>
        /// WARNING: Key status will be down while the player is crouched, even if they are not holding the key down.
        /// </summary>
        Crouch = 3,
        /// <summary>
        /// WARNING: Key status will be down while the player is prone, even if they are not holding the key down.
        /// </summary>
        Prone = 4,
        Sprint = 5,
        LeanLeft = 6,
        LeanRight = 7,
        /// <summary>
        /// Defaults to comma
        /// </summary>
        CodeHotkey1 = 9,
        /// <summary>
        /// Defaults to period
        /// </summary>
        CodeHotkey2 = 10,
        /// <summary>
        /// Defaults to forward slash
        /// </summary>
        CodeHotkey3 = 11,
        /// <summary>
        /// Defaults to semicolon
        /// </summary>
        CodeHotkey4 = 12,
        /// <summary>
        /// Defaults to quote
        /// </summary>
        CodeHotkey5 = 13,
    }
}