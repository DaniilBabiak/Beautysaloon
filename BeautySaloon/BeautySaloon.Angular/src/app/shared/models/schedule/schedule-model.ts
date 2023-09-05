import { WorkingDayModel } from "./working-day-model";

export interface ScheduleModel {
    id: number,
    masterId: number,
    workingDays: WorkingDayModel[]
}