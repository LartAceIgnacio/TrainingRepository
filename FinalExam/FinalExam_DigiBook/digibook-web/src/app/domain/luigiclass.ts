import { Luigi } from '../domain/luigi';
export class LuigiClass implements Luigi {

    constructor(public luigiId?, public firstName?, public lastName?
        , public luigiCount?) {

    }
}