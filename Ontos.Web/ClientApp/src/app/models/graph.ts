

export class Page {
  constructor(
    public id: string,
    public content: string,
    public type: string,
    public references: Reference[] = []) {
  }
}

export class PageType {
  constructor(
    public id: string,
    public label: string,
    public icon: string) {
  }

  public static UNKNOWN = new PageType("Unknown", "Unknown", "fas fa-question");
  public static THEORY = new PageType("Theory", "Theory", "fas fa-atom");
  public static CONCEPT = new PageType("Concept", "Concept", "far fa-lightbulb");

  static all() {
    return [
      this.UNKNOWN,
      this.THEORY,
      this.CONCEPT
    ];
  }

  public static default = PageType.UNKNOWN;

  public static parse(id: string) {
    return this.all().find(pageType => pageType.id == id);
  }
}


export class Reference {
  constructor(
    public id: number,
    public pageId: number,
    public expression: Expression) {
  }
}

export class Expression {
  constructor(
    public id: number,
    public language: string,
    public label: string) {
  }
}

export class Relation {
  constructor(
    public id: string,
    public type: string,
    public originId: string,
    public targetId: string) {
  }
}

export class RelatedPage {
  constructor(
    public id: string,
    public type: string,
    public originId: string,
    public target: Page) {
  }
}

export class RelationType {
  constructor(
    public label: string,
    public directed: boolean,
    public acyclic: boolean) {
  }

  public static INCLUSION = new RelationType('INCLUDE', true, true);
  public static INTERSECTION = new RelationType('INTERSECT', false, false);
}

export class DirectedRelationType {
  constructor(
    public label: string,
    public type: RelationType,
    public reversed: boolean) {
  }

  public static INCLUDES = new DirectedRelationType("Includes", RelationType.INCLUSION, false);
  public static INCLUDED_IN = new DirectedRelationType("Is included in", RelationType.INCLUSION, true);
  public static INTERSECTS = new DirectedRelationType("Intersects", RelationType.INTERSECTION, false);

  static all() {
    return [this.INCLUDES, this.INCLUDED_IN, this.INTERSECTS];
  }
}

export class NewPage {
  constructor(
    public content: string,
    public expression: NewExpression = null) {
  }
}

export class PageSearch {
  constructor(
    public language: string,
    public text: string) {
  }
}

export class PageSearchResult {
  constructor(
    public pageId: string,
    public score: number,
    public expressions: string[]) {
  }
}

export class NewExpression {
  constructor(
    public language: string,
    public label: string) {
  }
}

export class NewReference {
  constructor(
    public pageId: number,
    public expression: NewExpression) {
  }
}

export class NewRelation {
  constructor(
    public type: RelationType,
    public originId: string,
    public targetId: string) {
  }

  public static create(type: DirectedRelationType, originId: string, targetId: string) {
    if (type.reversed)
      [originId, targetId] = [targetId, originId];

    return new NewRelation(type.type, originId, targetId);
  }

  public toParams() {
    return {
      type: this.type.label, originId: this.originId, targetId: this.targetId,
    };
  }
}


export class NewRelatedPage {
  constructor(
    public type: DirectedRelationType,
    public expression: NewExpression,
    public content: string) {
  }

  public toParams() {
    return {
      type: this.type.type.label,
      reversed: this.type.reversed,
      expression: this.expression,
      content: this.content,
    };
  }
}

export class UpdatePage {
  constructor(
    public id: string,
    public content: string,
    public type: string) {
  }
}
