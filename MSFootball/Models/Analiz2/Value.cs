using System.Runtime.CompilerServices;

namespace MSFootball.Models.Analiz2
{
    public class Value<T>
    {
        protected T value;

        public Value() { }
        public Value(T value) => this.value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SetValue(T newValue) => this.value = newValue;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual T GetValue() => this.value;

        public static implicit operator Value<T>(T value) => new Value<T>(value);
        public static implicit operator T(Value<T> value) => value.value;

        public override string ToString() => value.ToString();
        public override bool Equals(object obj) => value.Equals(obj);
        public override int GetHashCode() => value.GetHashCode();
    }
}
