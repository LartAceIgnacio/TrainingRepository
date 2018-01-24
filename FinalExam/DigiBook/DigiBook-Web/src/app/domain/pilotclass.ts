import { Pilot } from "./pilot";

export class Pilotclass implements Pilot{

    constructor(public pilotId?, public firstName?, public middleName?,
            public lastName?, public dateOfBirth?, public yearsOfExperience?,
            public dateActivated?, public pilotCode?, public dateCreated?,
            public dateModified? ) {}
}