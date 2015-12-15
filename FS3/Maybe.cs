using System;

namespace FS3
{
    public class Maybe<TObject>
    {

        private TObject obj = default(TObject);
        private bool isNothing = true;

        private Maybe() { }

        private Maybe(TObject obj)
        {
            this.obj = obj;
            this.isNothing = false;
        }

        // A nothing
        public static Maybe<TObject> Zero()
        {
            return new Maybe<TObject>();
        }

        // A value
        public static Maybe<TObject> Bind(TObject obj)
        {
            return new Maybe<TObject>(obj);
        }

        // convert a Maybe<Maybe<TValue>> into a Maybe<TValue>
        public static Maybe<TObject> Join(Maybe<Maybe<TObject>> MaybeMaybe)
        {
            return MaybeMaybe.isNothing ? Zero() : MaybeMaybe.obj;
        }

        // functor
        public Maybe<TNewObject> FMap<TNewObject>(Func<TObject, TNewObject> functor)
        {
            return isNothing
                ? Maybe<TNewObject>.Zero()
                : Maybe<TNewObject>.Bind(functor(this.obj));
        }

        // applicative - maybe applies a maybe<f[m]> maybe<x[n]>
        // returns a Maybe 
        public Maybe<TNewObject> LiftA<TNewObject>(Maybe<Func<TObject, TNewObject>> applicative)
        {
            return Maybe<TNewObject>.Join(applicative.FMap(functor => this.FMap(functor)));
        }

        // monad - returns a joined monad of the result
        public Maybe<TNewObject> LiftM<TNewObject>(Func<TObject, Maybe<TNewObject>> monad)
        {
            return Maybe<TNewObject>.Join(this.FMap(monad));
        }

        public override string ToString()
        {
            return isNothing
                ? "Nothing<" + typeof(TObject).Name + ">"
                : "Just<" + typeof(TObject).Name + "> " + obj.ToString();
        }
    }
}