﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections;
using System.Xml.Serialization;


[System.Xml.Serialization.XmlRootAttribute("book")]
public partial class book 
{
    
    private bookChapter[] chaptersField;
   

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("chapter", IsNullable=false)]
    public bookChapter[] chapters {
        get {
            return this.chaptersField;
        }
        set {
            this.chaptersField = value;
        }
    }

    public IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}


[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapter {
    
    private bookChapterPage[] pagesField;
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("page", IsNullable=false)]
    public bookChapterPage[] pages {
        get {
            return this.pagesField;
        }
        set {
            this.pagesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPage {
    
    private bookChapterPageFrame[] framesField;
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("frame", IsNullable=false)]
    public bookChapterPageFrame[] frames {
        get {
            return this.framesField;
        }
        set {
            this.framesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrame {
    
    private bookChapterPageFrameScene sceneField;
    
    private bookChapterPageFrameContent[] contentsField;
    
    private string idField;
    
    private string bordertypeField;
    
    /// <remarks/>
    public bookChapterPageFrameScene scene {
        get {
            return this.sceneField;
        }
        set {
            this.sceneField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("content", IsNullable=false)]
    public bookChapterPageFrameContent[] contents {
        get {
            return this.contentsField;
        }
        set {
            this.contentsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string bordertype {
        get {
            return this.bordertypeField;
        }
        set {
            this.bordertypeField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameScene {
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContent {
    
    private bookChapterPageFrameContentObject[] objectsField;
    
    private bookChapterPageFrameContentChoice choiceField;
    
    private string targetField;
    
    private string typeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("object", IsNullable=false)]
    public bookChapterPageFrameContentObject[] objects {
        get {
            return this.objectsField;
        }
        set {
            this.objectsField = value;
        }
    }
    
    /// <remarks/>
    public bookChapterPageFrameContentChoice choice {
        get {
            return this.choiceField;
        }
        set {
            this.choiceField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string target {
        get {
            return this.targetField;
        }
        set {
            this.targetField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string type {
        get {
            return this.typeField;
        }
        set {
            this.typeField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentObject {
    
    private string typeField;
    
    private string idField;
    
    private string animationField;
    
    private string stringField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string type {
        get {
            return this.typeField;
        }
        set {
            this.typeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string animation {
        get {
            return this.animationField;
        }
        set {
            this.animationField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string @string {
        get {
            return this.stringField;
        }
        set {
            this.stringField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoice {
    
    private bookChapterPageFrameContentChoiceQuestion questionField;
    
    private bookChapterPageFrameContentChoicePositive positiveField;
    
    private bookChapterPageFrameContentChoiceNegative negativeField;
    
    private string ownerField;
    
    /// <remarks/>
    public bookChapterPageFrameContentChoiceQuestion question {
        get {
            return this.questionField;
        }
        set {
            this.questionField = value;
        }
    }
    
    /// <remarks/>
    public bookChapterPageFrameContentChoicePositive positive {
        get {
            return this.positiveField;
        }
        set {
            this.positiveField = value;
        }
    }
    
    /// <remarks/>
    public bookChapterPageFrameContentChoiceNegative negative {
        get {
            return this.negativeField;
        }
        set {
            this.negativeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string owner {
        get {
            return this.ownerField;
        }
        set {
            this.ownerField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoiceQuestion {
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoicePositive {
    
    private bookChapterPageFrameContentChoicePositiveText textField;
    
    private bookChapterPageFrameContentChoicePositiveResult resultField;
    
    /// <remarks/>
    public bookChapterPageFrameContentChoicePositiveText text {
        get {
            return this.textField;
        }
        set {
            this.textField = value;
        }
    }
    
    /// <remarks/>
    public bookChapterPageFrameContentChoicePositiveResult result {
        get {
            return this.resultField;
        }
        set {
            this.resultField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoicePositiveText {
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoicePositiveResult {
    
    private bookChapterPageFrameContentChoicePositiveResultSuccess successField;
    
    private bookChapterPageFrameContentChoicePositiveResultFailed failedField;
    
    /// <remarks/>
    public bookChapterPageFrameContentChoicePositiveResultSuccess success {
        get {
            return this.successField;
        }
        set {
            this.successField = value;
        }
    }
    
    /// <remarks/>
    public bookChapterPageFrameContentChoicePositiveResultFailed failed {
        get {
            return this.failedField;
        }
        set {
            this.failedField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoicePositiveResultSuccess {
    
    private string gotoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string @goto {
        get {
            return this.gotoField;
        }
        set {
            this.gotoField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoicePositiveResultFailed {
    
    private string gotoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string @goto {
        get {
            return this.gotoField;
        }
        set {
            this.gotoField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoiceNegative {
    
    private bookChapterPageFrameContentChoiceNegativeText textField;
    
    private bookChapterPageFrameContentChoiceNegativeResult resultField;
    
    /// <remarks/>
    public bookChapterPageFrameContentChoiceNegativeText text {
        get {
            return this.textField;
        }
        set {
            this.textField = value;
        }
    }
    
    /// <remarks/>
    public bookChapterPageFrameContentChoiceNegativeResult result {
        get {
            return this.resultField;
        }
        set {
            this.resultField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoiceNegativeText {
    
    private string idField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoiceNegativeResult {
    
    private bookChapterPageFrameContentChoiceNegativeResultSuccess successField;
    
    private bookChapterPageFrameContentChoiceNegativeResultFailed failedField;
    
    /// <remarks/>
    public bookChapterPageFrameContentChoiceNegativeResultSuccess success {
        get {
            return this.successField;
        }
        set {
            this.successField = value;
        }
    }
    
    /// <remarks/>
    public bookChapterPageFrameContentChoiceNegativeResultFailed failed {
        get {
            return this.failedField;
        }
        set {
            this.failedField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoiceNegativeResultSuccess {
    
    private string gotoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string @goto {
        get {
            return this.gotoField;
        }
        set {
            this.gotoField = value;
        }
    }
}
 
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class bookChapterPageFrameContentChoiceNegativeResultFailed {
    
    private string gotoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string @goto {
        get {
            return this.gotoField;
        }
        set {
            this.gotoField = value;
        }
    }
}
