using System;
using System.Reactive.Linq;
using SharpBrick.PoweredUp.Protocol;
using SharpBrick.PoweredUp.Protocol.Knowledge;
using SharpBrick.PoweredUp.Protocol.Messages;

namespace SharpBrick.PoweredUp
{
    public class MultiValueMode<TPayload> : Mode
    {
        public TPayload[] Raw { get; private set; }
        public TPayload[] SI { get; private set; }
        public TPayload[] Pct { get; private set; }

        public IObservable<Value<TPayload[]>> Observable { get; }

        public MultiValueMode(IPoweredUpProtocol protocol, PortModeInfo modeInfo, IObservable<PortValueData> modeValueObservable)
                : base(protocol, modeInfo, modeValueObservable)
        {
            Observable = CreateObservable();
            ObserveOnLocalProperty(Observable, v => Raw = v.Raw, v => SI = v.SI, v => Pct = v.Pct);
            ObserveForPropertyChanged(Observable, nameof(Raw), nameof(SI), nameof(Pct));
        }
        protected IObservable<Value<TPayload[]>> CreateObservable()
            => _modeValueObservable
                .Cast<PortValueData<TPayload>>()
                .Select(pvd => new Value<TPayload[]>()
                {
                    Raw = pvd.InputValues,
                    SI = pvd.SIInputValues,
                    Pct = pvd.PctInputValues,
                });
    }
}