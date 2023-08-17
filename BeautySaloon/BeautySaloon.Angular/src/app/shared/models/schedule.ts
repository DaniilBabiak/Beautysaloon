import { DayOff } from "./dayoff";
import { Master } from "./master";
import { WorkingDay } from "./working-day";

export interface Schedule {
    id: number | null;
    masterId: number | null;
    master: Master | null;
    workingDays: WorkingDay[] | null;
    dayOffs: DayOff[] | null;
  }