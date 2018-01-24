import { Pilot } from '../pilots/pilot';

export class PilotClass implements Pilot {

    constructor(public pilotId?,
        public firstName?,
        public middleName?,
        public lastName?,
        public dateOfBirth?,
        public dateCreated?,
        public yearsOfExperience?,
        public dateActivated?,
        public pilotCode?,
        ) {}
}
