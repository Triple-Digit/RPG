using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
    
    void Start()
    {
        RoomTemplate.Instance.rooms.Add(this.gameObject);
    }

    
}
