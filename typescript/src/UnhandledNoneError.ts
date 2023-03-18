export default class UnhandledNoneError extends Error {
    constructor() {
        super("Cannot access a None value for an option");
    }
}
