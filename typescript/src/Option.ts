import UnhandledNoneError from './UnhandledNoneError';
type Some<T> = T
type None = null | undefined

/**
 * Wraps a possible null or undefined value in a type for which the value is 'Some' if not null or undefined.
 * Allows for easier null handling 
 */
export default class Option<T> {
    private readonly _value: Some<T> | None;
    public get value(): Some<T> {
        if (this._value === null || this._value === undefined) throw new UnhandledNoneError();
        return this._value;
    }

    constructor(value: Some<T> | None = null) {
        this._value = value;
    }

    public isSome(): boolean {
        return this._value !== null && this._value !== undefined;
    }

    public isNone(): boolean {
        return !this.isSome();
    }

    public match<TReturn>(
        someFunc: (value: Some<T>) => TReturn,
        noneFunc: () => TReturn
    ): TReturn {
        return this.isSome() ? someFunc(this._value!) : noneFunc();
    }

    public act(
        someFunc: (value: Some<T>) => void,
        noneFunc: () => void
    ): void {
        this.isSome() ? someFunc(this._value!) : noneFunc();
    }

    public bind<TReturn>(bindFunc: (value: Some<T>) => Option<TReturn>): Option<TReturn> {
        return this.match(bindFunc, () => Option.none<TReturn>());
    }

    public map<TReturn>(mapFunc: (value: Some<T>) => TReturn): Option<TReturn> {
        return this.bind((someValue) => Option.some<TReturn>(mapFunc(someValue)));
    }

    public filter(predicate: (value: Some<T>) => boolean): Option<T> {
        return this.match<Option<T>>(
            (someValue) => predicate(someValue) ? Option.some(someValue) : Option.none<T>(),
            () => Option.none<T>()
        );
    }

    public contains(condition: (value: Some<T>) => boolean): Option<T> {
        return this.match<Option<T>>(
            (someValue) => condition(someValue) ? Option.some(someValue) : Option.none<T>(),
            () => Option.none<T>()
        );
    }

    public valueOrDefault(defaultValue: T): T {
        return this.match((someValue) => someValue, () => defaultValue);
    }

    public toArray(): T[] {
        return this.match(
            (someValue) => [someValue],
            () => [] as T[]
        );
    }

    public async toPromise(): Promise<Option<T>> {
        return this;
    }

    /**
     * Required for serialisation
     * @returns Base value if option isSome, else explicit null
     */
    public toJSON(key?: string) {
        return this.isSome() ? this._value! : null;
    }

    public static some<TInit>(value: TInit): Option<TInit> {
        return new Option(value);
    }

    public static none<TInit>(): Option<TInit> {
        return new Option<TInit>();
    }
}