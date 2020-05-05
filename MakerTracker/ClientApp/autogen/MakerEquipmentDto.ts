﻿// AutoGenerated File

/** Data transfer object for an equipment assignment to a maker */
export class MakerEquipmentDto {

    /** Gets or sets the identifier. */
    id: number;

    /** Gets or sets the maker identifier. */
    makerId: number;

    /** Gets or sets the equipment identifier. */
    equipmentId: number;

    /** Gets or sets the name of the equipment. */
    equipmentName: string;

    /** Gets or sets the manufacturer of the equipment owned. */
    manufacturer: string;

    /** Gets or sets the model number of the equipment owned. */
    modelNumber: string;

    /** Initializes a new instance of the MakerEquipmentDto class **/
    public constructor(init?: Partial<MakerEquipmentDto>) {
        Object.assign(this, init);
    }
}
