import Option from "../src/Option";
import UnhandledNoneError from '../src/UnhandledNoneError';

describe('Option.isSome & Option.isNone', () => {
    const testCases = {
        some: [
            new Option(3),
            new Option(""),
            Option.some(new Date()),
            new Option(false)
        ],
        none: [
            new Option<number>(),
            Option.none<string>(),
            new Option<Date>(null),
            new Option<boolean>(undefined)
        ]
    }

    it('IsSome - should be true if value not null or undefined', () => testCases.some.forEach(someTestCase => expect(someTestCase.isSome()).toBe(true)));
    it('IsSome - should be false if value null or undefined', () => testCases.none.forEach(noneTestCase => expect(noneTestCase.isSome()).toBe(false)))
    it('IsNone - should be false if value not null or undefined', () => testCases.some.forEach(someTestCase => expect(someTestCase.isNone()).toBe(false)));
    it('IsSome - should be false if value null or undefined', () => testCases.none.forEach(noneTestCase => expect(noneTestCase.isNone()).toBe(true)))
})

describe('Option.value', () => {
    const someValue = 10;
    const testCases = {
        some: Option.some(someValue),
        none: Option.none()
    }

    it('should return inner value if some', () => expect(testCases.some.value).toBe(someValue))
    it('should throw indicative error if none', () => expect(() => testCases.none.value).toThrowError(new UnhandledNoneError()))
})

describe('Option.valueOrDefault', () => {
    const someValue = 10;
    const testCases = {
        some: Option.some(someValue),
        none: Option.none<number>()
    }

    it('should return inner value if some', () => expect(testCases.some.valueOrDefault(someValue + 1)).toBe(someValue))
    it('should throw indicative error if none', () => expect(testCases.none.valueOrDefault(someValue + 1)).toBe(someValue + 1))
});

describe('Option.match', () => {
    const someValue = 10;
    const testCases = {
        some: Option.some(someValue),
        none: Option.none<number>()
    }
    const matchFunctions = {
        some: (input: number) => input + 1,
        none: () => -1
    }
    
    it('should match some value if some', () => expect(testCases.some.match(matchFunctions.some, matchFunctions.none)).toBe(someValue + 1));
    it('should match none value if none', () => expect(testCases.none.match(matchFunctions.some, matchFunctions.none)).toBe(-1));
});

describe('Option.act', () => {
    const someValue = 10;
    let actValue = 0;
    const testCases = {
        some: Option.some(someValue),
        none: Option.none<number>()
    }
    const actFunctions = {
        some: (input: number) => actValue = input + 1,
        none: () => actValue = -1
    }

    beforeEach(() => actValue = 0);
    
    it('should act with some value if some', () => {
        testCases.some.act(actFunctions.some, actFunctions.none);
        expect(actValue).toBe(someValue + 1);
    });
    it('should act with none if none', () => {
        testCases.none.act(actFunctions.some, actFunctions.none);
        expect(actValue).toBe(-1);
    });
});