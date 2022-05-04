using System;

namespace BookShop.Services
{
    public delegate void UnregisterCallback<TE>(EventHandler<TE> eventHandler)
    where TE : EventArgs;

    public interface IWeakEventHandler<TE>
        where TE : EventArgs
    {
        EventHandler<TE> Handler { get; }
    }

    public class WeakEventHandler<T, TE> : IWeakEventHandler<TE>
        where T : class
        where TE : EventArgs
    {
        private delegate void OpenEventHandler(T @this, object sender, TE e);

        private readonly WeakReference mTargetRef;
        private readonly OpenEventHandler mOpenHandler;
        private readonly EventHandler<TE> mHandler;
        private UnregisterCallback<TE> mUnregister;

        public WeakEventHandler(EventHandler<TE> eventHandler,
                                    UnregisterCallback<TE> unregister)
        {
            mTargetRef = new WeakReference(eventHandler.Target);

            mOpenHandler = (OpenEventHandler)Delegate.CreateDelegate(
                               typeof(OpenEventHandler), null, eventHandler.Method);

            mHandler = Invoke;
            mUnregister = unregister;
        }

        public void Invoke(object sender, TE e)
        {
            T target = (T)mTargetRef.Target;

            if (target != null)
                mOpenHandler.Invoke(target, sender, e);
            else if (mUnregister != null)
            {
                mUnregister(mHandler);
                mUnregister = null;
            }
        }

        public EventHandler<TE> Handler
        {
            get { return mHandler; }
        }

        public static implicit operator EventHandler<TE>(WeakEventHandler<T, TE> weh)
        {
            return weh.mHandler;
        }
    }

    public static class EventHandlerUtils
    {
        public static EventHandler<TE> MakeWeak<TE>(this EventHandler<TE> eventHandler,
                                                        UnregisterCallback<TE> unregister)
            where TE : EventArgs
        {
            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            if (eventHandler.Method.IsStatic || eventHandler.Target == null)
                throw new ArgumentException("Only instance methods are supported.",
                                                "eventHandler");

            var wehType = typeof(WeakEventHandler<,>).MakeGenericType(
                              eventHandler.Method.DeclaringType, typeof(TE));

            var wehConstructor = wehType.GetConstructor(new Type[]
                                 {
                                 typeof(EventHandler<TE>), typeof(UnregisterCallback<TE>)
                                 });

            IWeakEventHandler<TE> weh = (IWeakEventHandler<TE>)wehConstructor.Invoke(
                                            new object[] { eventHandler, unregister });

            return weh.Handler;
        }
    }
}
