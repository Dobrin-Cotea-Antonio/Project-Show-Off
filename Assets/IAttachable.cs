using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachable{
    public void Attach(ToolBelt pBelt);
    public void Detach(ToolBelt pBelt);
    public ToolBelt AttachedToolbelt();
}
