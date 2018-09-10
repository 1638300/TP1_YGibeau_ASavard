using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Pickables.Types
{
    //BEN_CORRECTION : Le nom du fichier est pas le même que le nom de l'enum....
    //BEN_REVIEW : C'est le seul fichier dans ce dossier. Est-ce vraiment nécessaire ?
    public enum PickableTypes
    {
        Weapon = 0x0000001,
        Util = 0x0000010,
        Shotgun = 0x0000101,
        Uzi = 0x0001001,
        Medkit = 0x0000110
  }
}
