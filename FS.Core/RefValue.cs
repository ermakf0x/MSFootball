namespace FS.Core
{
    public sealed class RefValue<TValue>
    {
        public TValue Value { get; set; }

        public RefValue() => Value = default;
        public RefValue(TValue value) => Value = value;

        public static implicit operator RefValue<TValue>(TValue value) => new RefValue<TValue>(value);
        public static implicit operator TValue(RefValue<TValue> value) => value.Value;
        public static bool operator ==(RefValue<TValue> fValue, TValue tValue) => fValue.Value.Equals(tValue);
        public static bool operator !=(RefValue<TValue> fValue, TValue tValue) => !fValue.Value.Equals(tValue);

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => Value.Equals(obj);
        public override int GetHashCode() => Value.GetHashCode();
    }
}
