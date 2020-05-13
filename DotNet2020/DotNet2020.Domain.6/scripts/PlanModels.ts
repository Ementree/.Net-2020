class Project {
    id: number;
    name: string;
    statusId: number;
    periods: Period[];
}

class Period {
    capacity: number;
    date: Date;
    resources: ResourceCapacity[];

    constructor(capacity?: number, date?: Date, resources?: ResourceCapacity[]) {
        this.capacity = capacity;
        this.date = date;
        this.resources = resources;
    }
}

class ResourceCapacity {
    constructor(id: number, name: string, capacity: number) {
        this.id = id;
        this.name = name;
        this.capacity = capacity;
    }

    id: number;
    name: string;
    capacity: number;
}

class ProjectStatus {
    id: number;
    status: string;

    constructor(id: number, status: string) {
        this.id = id;
        this.status = status;
    }
}