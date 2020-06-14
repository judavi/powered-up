using System;
using SharpBrick.PoweredUp.Devices;
using SharpBrick.PoweredUp.Protocol.Messages;
using SharpBrick.PoweredUp.Utils;
using Xunit;

namespace SharpBrick.PoweredUp.Protocol.Formatter
{
    public class PortOutputCommandEncoderTest
    {

        [Theory]
        [InlineData("09-00-81-00-11-05-B8-0B-01", 0, 3000, SpeedProfiles.AccelerationProfile)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandSetAccTimeMessage(string expectedData, byte port, ushort time, SpeedProfiles profile)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandSetAccTimeMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                Time = time,
                Profile = profile,
            });

        [Theory]
        [InlineData("09-00-81-00-11-06-E8-03-02", 0, 1000, SpeedProfiles.DeccelerationProfile)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandSetDecTimeMessage(string expectedData, byte port, ushort time, SpeedProfiles profile)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandSetDecTimeMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                Time = time,
                Profile = profile,
            });

        [Theory]
        [InlineData("09-00-81-00-11-07-64-5A-03", 0, 100, 90, SpeedProfiles.AccelerationProfile | SpeedProfiles.DeccelerationProfile)]
        [InlineData("09-00-81-00-11-07-7F-5A-03", 0, 127, 90, SpeedProfiles.AccelerationProfile | SpeedProfiles.DeccelerationProfile)]
        [InlineData("09-00-81-00-11-07-9C-5A-03", 0, -100, 90, SpeedProfiles.AccelerationProfile | SpeedProfiles.DeccelerationProfile)]
        [InlineData("09-00-81-00-11-07-00-5A-03", 0, 0, 90, SpeedProfiles.AccelerationProfile | SpeedProfiles.DeccelerationProfile)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandStartSpeedMessage(string expectedData, byte port, sbyte speed, byte maxPower, SpeedProfiles profile)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandStartSpeedMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                Speed = speed,
                MaxPower = maxPower,
                Profile = profile,
            });

        [Theory]
        [InlineData("0C-00-81-00-11-09-B8-0B-5A-64-7E-00", 0, 3000, 90, 100, SpecialSpeed.Hold, SpeedProfiles.None)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandStartSpeedForTimeMessage(string expectedData, byte port, ushort time, sbyte speed, byte maxPower, SpecialSpeed endState, SpeedProfiles profile)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandStartSpeedForTimeMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                Time = time,
                Speed = speed,
                MaxPower = maxPower,
                EndState = endState,
                Profile = profile,
            });

        [Theory]
        [InlineData("0E-00-81-00-11-0B-B4-00-00-00-F6-64-7F-00", 0, 180, -10, 100, SpecialSpeed.Brake, SpeedProfiles.None)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandStartSpeedForDegreesMessage(string expectedData, byte port, uint degrees, sbyte speed, byte maxPower, SpecialSpeed endState, SpeedProfiles profile)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandStartSpeedForDegreesMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                Degrees = degrees,
                Speed = speed,
                MaxPower = maxPower,
                EndState = endState,
                Profile = profile,
            });

        [Theory]
        [InlineData("0E-00-81-00-11-0D-2D-00-00-00-0A-64-7F-00", 0, 45, 10, 100, SpecialSpeed.Brake, SpeedProfiles.None)]
        [InlineData("0E-00-81-00-11-0D-D3-FF-FF-FF-0A-64-7F-00", 0, -45, 10, 100, SpecialSpeed.Brake, SpeedProfiles.None)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandGotoAbsolutePositionMessage(string expectedData, byte port, int absPosition, sbyte speed, byte maxPower, SpecialSpeed endState, SpeedProfiles profile)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandGotoAbsolutePositionMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                AbsolutePosition = absPosition,
                Speed = speed,
                MaxPower = maxPower,
                EndState = endState,
                Profile = profile,
            });

        [Theory]
        [InlineData("08-00-81-00-11-51-00-64", 0, 100)]
        [InlineData("08-00-81-00-11-51-00-7F", 0, 127)]
        [InlineData("08-00-81-00-11-51-00-9C", 0, -100)]
        [InlineData("08-00-81-00-11-51-00-00", 0, 0)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandStartPowerMessage(string expectedData, byte port, sbyte power)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandStartPowerMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                Power = power
            });

        [Theory]
        [InlineData("0A-00-81-32-11-51-01-00-FF-00", 50, 0x00, 0xFF, 0x00)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandSetRgbColorNo2Message(string expectedData, byte port, byte r, byte g, byte b)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandSetRgbColorNo2Message()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                RedColor = r,
                GreenColor = g,
                BlueColor = b,
            });

        [Theory]
        [InlineData("08-00-81-32-11-51-00-05", 50, PoweredUpColor.Cyan)]
        public void PortOutputCommandEncoder_Encode_PortOutputCommandSetRgbColorNoMessage(string expectedData, byte port, PoweredUpColor color)
            => PortOutputCommandEncoder_Encode(expectedData, new PortOutputCommandSetRgbColorNoMessage()
            {
                PortId = port,
                StartupInformation = PortOutputCommandStartupInformation.ExecuteImmediately,
                CompletionInformation = PortOutputCommandCompletionInformation.CommandFeedback,
                ColorNo = color,
            });

        private void PortOutputCommandEncoder_Encode(string expectedDataAsString, PoweredUpMessage message)
        {
            // act
            var data = MessageEncoder.Encode(message, null);

            // assert
            Assert.Equal(expectedDataAsString, BytesStringUtil.DataToString(data));
        }
    }
}