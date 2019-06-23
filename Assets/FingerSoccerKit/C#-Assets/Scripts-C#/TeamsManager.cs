using UnityEngine;
using System.Collections;

public class TeamsManager : MonoBehaviour {


	///*************************************************************************///
	/// Main Teams manager.
	/// You can define new teams here.
	///*************************************************************************///

	public static int teams = 60;		//total number of available teams

	//Power differs from 1 (weakest) to 10 (strongest) (base power: 35)
	//additional time differs from 1 seconds (little) to 10 seconds (too much) (base time: 15 seconds)

	public static Vector2 getTeamSettings(int _teamID) {
		Vector2 settings = Vector2.zero;
		switch (_teamID) {

            //National

            //Iran
		case 0:
			settings = new Vector2(5, 7);
			break;
            //Sweden
            case 1:
			settings = new Vector2(2, 9);
			break;
            //Uruguay
            case 2:
			settings = new Vector2(8, 6);
			break;
            //Switzerland
            case 3:
			settings = new Vector2(7, 5);
			break;
            //Slovakia
            case 4:
			settings = new Vector2(3, 8);
			break;
            //Scotland
            case 5:
			settings = new Vector2(3, 10);
			break;
            //Portugal
            case 6:
			settings = new Vector2(9, 4);
			break;
            //Poland
            case 7:
			settings = new Vector2(5, 8);
			break;
            //Peru
            case 8:
			settings = new Vector2(4, 8);
			break;
            //Mexico
            case 9:
			settings = new Vector2(5, 7);
			break;
            //Kolumbien
            case 10:
			settings = new Vector2(7, 5);
			break;
            //Iceland
            case 11:
                settings = new Vector2(3, 7);
                break;
            //England
            case 12:
                settings = new Vector2(9, 3);
                break;
            //Belgium
            case 13:
                settings = new Vector2(10, 2);
                break;
            //Chile
            case 14:
                settings = new Vector2(6, 3);
                break;
            //Denmark
            case 15:
                settings = new Vector2(7, 4);
                break;
            //Croatia
            case 16:
                settings = new Vector2(8, 3);
                break;
            //Brazil
            case 17:
                settings = new Vector2(7, 5);
                break;
            //Argentina
            case 18:
                settings = new Vector2(5, 6);
                break;
            //Germany
            case 19:
                settings = new Vector2(4, 8);
                break;
            //Finland
            case 20:
                settings = new Vector2(5, 6);
                break;
            //Italy
            case 21:
                settings = new Vector2(8, 3);
                break;
            //Japan
            case 22:
                settings = new Vector2(3, 9);
                break;
            //Netherlands
            case 23:
                settings = new Vector2(4, 6);
                break;
            //Russia
            case 24:
                settings = new Vector2(5, 7);
                break;
            //South Korea
            case 25:
                settings = new Vector2(4, 9);
                break;
            //Spain
            case 26:
                settings = new Vector2(9, 3);
                break;
            //USA
            case 27:
                settings = new Vector2(3, 10);
                break;

            // Iran FC

            //Perspolis
            case 28:
                settings = new Vector2(9, 4);
                break;
            //Sepahan
            case 29:
                settings = new Vector2(8, 5);
                break;
            //Esteghlal
            case 30:
                settings = new Vector2(9, 3);
                break;
            //Padideh Shahr Khodro
            case 31:
                settings = new Vector2(8, 4);
                break;
            //Tractorsazi
            case 32:
                settings = new Vector2(7, 5);
                break;
            //Zob Ahan
            case 33:
                settings = new Vector2(8, 4);
                break;
            //Saipa
            case 34:
                settings = new Vector2(7, 6);
                break;
            //Foolad
            case 35:
                settings = new Vector2(6, 4);
                break;
            //Sanat Naft Abadan
            case 36:
                settings = new Vector2(6, 6);
                break;
            //Nassagi Mazandaran
            case 37:
                settings = new Vector2(5, 6);
                break;
            //Peykan
            case 38:
                settings = new Vector2(8, 4);
                break;
            //Pars Jonobi Jam
            case 39:
                settings = new Vector2(4, 7);
                break;
            //Mashin Saazi Tabriz
            case 40:
                settings = new Vector2(3, 6);
                break;
            //Naft Masjed Soleiman
            case 41:
                settings = new Vector2(2, 8);
                break;
            //Sepid Rood Rasht
            case 42:
                settings = new Vector2(3, 7);
                break;
            //Esteghlal Khuzestan
            case 43:
                settings = new Vector2(4, 7);
                break;

            //World FC

            //Arsenal
            case 44:
                settings = new Vector2(2, 9);
                break;
            //Atletico De Madrid
            case 45:
                settings = new Vector2(7, 2);
                break;
            //Barcelona
            case 46:
                settings = new Vector2(9, 3);
                break;
            //Bayern Munchen
            case 47:
                settings = new Vector2(8, 4);
                break;
            //Chelsea
            case 48:
                settings = new Vector2(3, 10);
                break;
            //Inter Milan
            case 49:
                settings = new Vector2(2, 5);
                break;
            //Juventus
            case 50:
                settings = new Vector2(10, 3);
                break;
            //Liverpool
            case 51:
                settings = new Vector2(6, 6);
                break;
            //Manchester City
            case 52:
                settings = new Vector2(7, 5);
                break;
            //Manchester United
            case 53:
                settings = new Vector2(4, 7);
                break;
            //Napoli
            case 54:
                settings = new Vector2(5, 8);
                break;
            //Paris Saint Germain
            case 55:
                settings = new Vector2(6, 4);
                break;
            //Real Madrid
            case 56:
                settings = new Vector2(9, 3);
                break;
            //Roma
            case 57:
                settings = new Vector2(4, 6);
                break;
            //Tottenham
            case 58:
                settings = new Vector2(5, 7);
                break;
            //Villarreal
            case 59:
                settings = new Vector2(3, 8);
                break;

            default:
			settings = new Vector2(3, 3);
			break;
		}

		return settings;
	}

}