using ExitGames.Client.Photon;
using UnityEngine;


//직렬화 : 어떤 오브젝트를 바이트 데이터로 변환 처리 하는 것
//역직렬화: 바이트 데이터를 다시 원본 오브젝트로 변환 처리 하는 것

//color( hexadecimal ) 값은 4바이트
public class ColorSerialization {
    private static byte[] colorMemory = new byte[4 * 4];

    public static short SerializeColor(StreamBuffer outStream, object targetObject) {
        Color color = (Color) targetObject;

        lock (colorMemory)  //다른 곳에서 접근하지 못하게 잠금처리
        {   
            byte[] bytes = colorMemory;
            int index = 0;

            //한번에 모든 값을 보낼 수 없어서 ex) rgb 값을 알파포함 4개로 나눠서 보냄
            Protocol.Serialize(color.r, bytes, ref index);      
            Protocol.Serialize(color.g, bytes, ref index);
            Protocol.Serialize(color.b, bytes, ref index);
            Protocol.Serialize(color.a, bytes, ref index);
            outStream.Write(bytes, 0, 4*4);
        }

        return 4 * 4;
    }

    public static object DeserializeColor(StreamBuffer inStream, short length)  {
        Color color = new Color();
  
        lock (colorMemory)
        {
            inStream.Read(colorMemory, 0, 4 * 4);
            int index = 0;
            
            //나눠져서 받아온 값을 다시 rgb값 을 하나로 만드는 과정
            Protocol.Deserialize(out color.r,colorMemory, ref index);
            Protocol.Deserialize(out color.g,colorMemory, ref index);
            Protocol.Deserialize(out color.b,colorMemory, ref index);
            Protocol.Deserialize(out color.a,colorMemory, ref index);
        }
        
        return color;
    }
}