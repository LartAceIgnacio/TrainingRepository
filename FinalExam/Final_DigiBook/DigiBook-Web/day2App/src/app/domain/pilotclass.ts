import { Pilot } from '../domain/pilot';
export class PilotClass implements Pilot{

    constructor(
        public pilotId?,
        public firstName?,
        public middleName?,
        public middleInitial?,
        public lastName?,
        public birthDate?,
        public yearsOfExperience?,
        public dateActivated?,
        public pilotCode?){}
}