﻿using HouseofCat.Gremlins.Models.System;
using HouseofCat.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using static HouseofCat.Gremlins.Utilities.Helpers.ExceptionHelpers;

namespace HouseofCat.Gremlins.General
{
    public static class ExceptionGremlin
    {
        private static ThreadSafeRandomNumberGenerator _random = new ThreadSafeRandomNumberGenerator();

        #region General Exceptions

        public static async Task ThrowsSystemExceptionAsync()
        {
            await ThrowsRandomSystemExceptionAsync();
        }

        public static async Task ThrowsNetworkExceptionAsync()
        {
            await ThrowsRandomNetworkExceptionAsync();
        }

        public static async Task ThrowsSqlExceptionAsync()
        {
            await ThrowsRandomSqlExceptionAsync();
        }

        public static async Task ThrowsRandomExceptionAsync()
        {
            switch (_random.Next(1, 3))
            {
                case 1: await ThrowsRandomSystemExceptionAsync(); break;
                case 2: await ThrowsRandomNetworkExceptionAsync(); break;
                case 3: await ThrowsRandomSqlExceptionAsync(); break;
                default: break;
            }
        }

        #endregion

        #region Contextual Exceptions

        private static readonly ConcurrentDictionary<int, ExceptionTarget> _intExceptionCache = new ConcurrentDictionary<int, ExceptionTarget>();
        private static readonly ConcurrentDictionary<long, ExceptionTarget> _longExceptionCache = new ConcurrentDictionary<long, ExceptionTarget>();
        private static readonly ConcurrentDictionary<string, ExceptionTarget> _stringExceptionCache = new ConcurrentDictionary<string, ExceptionTarget>();

        public static async Task RollsTheDiceAsync(object input, ExceptionTarget target = null)
        {
            switch (input)
            {
                case int userInput: await HandleIntAsync(userInput, target); break;
                case long userInput: await HandleLongAsync(userInput, target); break;
                case string userInput: await HandleStringAsync(userInput, target); break;
                default: break;
            }
        }

        public static async Task RemoveTargetAsync(object input)
        {
            switch (input)
            {
                case int userInput: await RemoveIntAsync(userInput); break;
                case long userInput: await RemoveLongAsync(userInput); break;
                case string userInput: await RemoveStringAsync(userInput); break;
                default: break;
            }
        }

        #region Contextual Helpers

        private static async Task HandleIntAsync(int input, ExceptionTarget target = null, bool allowContextualCleanup = false)
        {
            var exTarget = await GetOrAddIntCachedExecptionTargetAsync(input, target);

            if (exTarget.FailureCount < exTarget.FailureCountMax)
            {
                var throwException = false;

                // Can We Throw Exception?
                if (exTarget.GuaranteeFailure
                    || exTarget.AllowConsecutiveFailure
                    || (!exTarget.AllowConsecutiveFailure && !exTarget.LastIterationFailed))
                {
                    // Will We Throw Exception?
                    throwException = await ShouldWeThrowExceptionAsync(input);

                    if (throwException) // The Math Gods Say Yes!
                    {
                        exTarget.FailureCount++;
                        exTarget.LastIterationFailed = true;
                    }
                    else
                    { exTarget.LastIterationFailed = false; }
                }
                else
                { exTarget.LastIterationFailed = false; }

                // Update Cache
                _intExceptionCache[input] = exTarget;

                if (throwException) { await ThrowsRandomExceptionAsync(); }
            }
            else if (allowContextualCleanup) // We are finished with this input! Allows it come back in!
            { _intExceptionCache.TryRemove(input, out ExceptionTarget outTarget); }
        }

        private static async Task HandleLongAsync(long input, ExceptionTarget target = null, bool allowContextualCleanup = false)
        {
            var exTarget = await GetOrAddLongCachedExecptionTargetAsync(input, target);

            if (exTarget.FailureCount < exTarget.FailureCountMax)
            {
                var throwException = false;

                // Can We Throw Exception?
                if (exTarget.GuaranteeFailure
                    || exTarget.AllowConsecutiveFailure
                    || (!exTarget.AllowConsecutiveFailure && !exTarget.LastIterationFailed))
                {
                    // Will We Throw Exception?
                    throwException = await ShouldWeThrowExceptionAsync(input);

                    if (throwException) // The Math Gods Say Yes!
                    {
                        exTarget.FailureCount++;
                        exTarget.LastIterationFailed = true;
                    }
                    else
                    { exTarget.LastIterationFailed = false; }
                }
                else
                { exTarget.LastIterationFailed = false; }

                // Update Cache
                _longExceptionCache[input] = exTarget;

                if (throwException) { await ThrowsRandomExceptionAsync(); }
            }
            else if (allowContextualCleanup) // We are finished with this input! Allows it come back in!
            { _longExceptionCache.TryRemove(input, out ExceptionTarget outTarget); }

        }

        private static async Task HandleStringAsync(string input, ExceptionTarget target = null, bool allowContextualCleanup = false)
        {
            var exTarget = await GetOrAddStringCachedExecptionTargetAsync(input, target);

            if (exTarget.FailureCount < exTarget.FailureCountMax)
            {
                var throwException = false;

                // Can We Throw Exception?
                if (exTarget.GuaranteeFailure
                    || exTarget.AllowConsecutiveFailure
                    || (!exTarget.AllowConsecutiveFailure && !exTarget.LastIterationFailed))
                {
                    // Will We Throw Exception?
                    throwException = await ShouldWeThrowExceptionAsync(input);

                    if (throwException) // The Math Gods Say Yes!
                    {
                        exTarget.FailureCount++;
                        exTarget.LastIterationFailed = true;
                    }
                    else
                    { exTarget.LastIterationFailed = false; }
                }
                else
                { exTarget.LastIterationFailed = false; }

                // Update Cache
                _stringExceptionCache[input] = exTarget;

                if (throwException) { await ThrowsRandomExceptionAsync(); }
            }
            else if (allowContextualCleanup) // We are finished with this input! Allows it come back in!
            { _stringExceptionCache.TryRemove(input, out ExceptionTarget outTarget); }
        }

        private static Task RemoveIntAsync(int input)
        {
            _intExceptionCache.TryRemove(input, out ExceptionTarget value);

            return Task.CompletedTask;
        }

        private static Task RemoveLongAsync(long input)
        {
            _longExceptionCache.TryRemove(input, out ExceptionTarget value);

            return Task.CompletedTask;
        }

        private static Task RemoveStringAsync(string input)
        {
            _stringExceptionCache.TryRemove(input, out ExceptionTarget value);

            return Task.CompletedTask;
        }

        private static Task<ExceptionTarget> GetOrAddIntCachedExecptionTargetAsync(int input, ExceptionTarget target = null)
        {
            ExceptionTarget exTarget = null;

            if (_intExceptionCache.ContainsKey(input))
            { exTarget = _intExceptionCache[input]; }
            else if (target is null)
            {
                exTarget = new ExceptionTarget
                {
                    AllowConsecutiveFailure = true,
                    FailureCount = 0,
                    FailureCountMax = 5,
                    GuaranteeFailure = false
                };

                _intExceptionCache[input] = exTarget;
            }
            else
            { _intExceptionCache[input] = exTarget = target; }

            return Task.FromResult(exTarget);
        }

        private static Task<ExceptionTarget> GetOrAddLongCachedExecptionTargetAsync(long input, ExceptionTarget target = null)
        {
            ExceptionTarget exTarget = null;

            if (_longExceptionCache.ContainsKey(input))
            { exTarget = _longExceptionCache[input]; }
            else if (target is null)
            {
                exTarget = new ExceptionTarget
                {
                    AllowConsecutiveFailure = true,
                    FailureCount = 0,
                    FailureCountMax = 5,
                    GuaranteeFailure = false
                };

                _longExceptionCache[input] = exTarget;
            }
            else
            { _longExceptionCache[input] = exTarget = target; }

            return Task.FromResult(exTarget);
        }

        private static Task<ExceptionTarget> GetOrAddStringCachedExecptionTargetAsync(string input, ExceptionTarget target = null)
        {
            ExceptionTarget exTarget = null;

            if (_stringExceptionCache.ContainsKey(input))
            { exTarget = _stringExceptionCache[input]; }
            else if (target is null)
            {
                exTarget = new ExceptionTarget
                {
                    AllowConsecutiveFailure = true,
                    FailureCount = 0,
                    FailureCountMax = 5,
                    GuaranteeFailure = false
                };

                _stringExceptionCache[input] = exTarget;
            }
            else
            { _stringExceptionCache[input] = exTarget = target; }

            return Task.FromResult(exTarget);
        }

        private static Task<bool> ShouldWeThrowExceptionAsync(int input)
        {
            var throwException = false;
            var randomMod = _random.Next(1, 4);

            if (input % randomMod == 0)
            { throwException = true; }

            return Task.FromResult(throwException);
        }

        private static Task<bool> ShouldWeThrowExceptionAsync(long input)
        {
            var throwException = false;
            var randomMod = _random.Next(1, 4);

            if (input % randomMod == 0)
            { throwException = true; }

            return Task.FromResult(throwException);
        }

        private static Task<bool> ShouldWeThrowExceptionAsync(string input)
        {
            var throwException = false;
            var randomNumber = _random.Next(0, 9);

            if (input.EndsWith(randomNumber.ToString()) || input.StartsWith(randomNumber.ToString()))
            { throwException = true; }

            return Task.FromResult(throwException);
        }

        #endregion

        #endregion
    }
}
