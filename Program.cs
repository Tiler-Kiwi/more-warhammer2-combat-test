/*
 * Created by SharpDevelop.
 * User: adam.moseman
 * Date: 6/10/2018
 * Time: 1:04 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace simplecombattest
{
	class Program
	{
		public static void Main(string[] args)
		{
			Random RNG = new Random();
			Fighter Sister = new Program.Fighter(RNG,"Sister", 40,60,563, (double).2, 27, 5, 12, true, false);
			Fighter DeathRunner = new Program.Fighter(RNG,"DeathRunner", 46, 68, 458, (double).3,8, 20, 22, false, true);
			Fighter IronBreaker = new Program.Fighter(RNG,"Ironbreaker", 30, 68, 720, 0,22, 125, 10);
			Fighter Swordmaster = new Program.Fighter(RNG,"Swordmaster",60,36,600, 0, 15,90,35);
			Fighter StormVerminHal = new Program.Fighter(RNG,"StormVermin(H)",22,30,744, 0, 9,90,18);
			Fighter StormVermin = new Program.Fighter(RNG,"StormVermin(S)",36,36,744, 0, 25,90,9);
			Fighter ChaosWarrior = new Program.Fighter(RNG, "Chaos Warrior", 36,44,675,0,26,100,10);
			Fighter StormVermin2 = new Program.Fighter(RNG,"SuperStormVermin(S)",36,36,744, 0, 25,90,9, true, true);
			Fighter SaurusWarrior = new Program.Fighter(RNG, "SaurusWarrior",28,28,765,0,34,60,18);
			Fighter SaurusWarrior2 = new Program.Fighter(RNG, "SaurusWarrior(S)",28,34,765,0,34,60,18);
			Fighter Chosen = new Program.Fighter(RNG, "Chosen", 46, 60, 792, 0, 32, 120, 12);
			Fighter PhoenixGuard = new Program.Fighter(RNG, "PhoenixGuard", 38,40,600,(double).3,11,100,25);
			Fighter PlagueMonk = new Program.Fighter(RNG, "Plague Monk", 43, 24, 684, (double).15,29, 10, 10);
			Fighter BleakSword = new Program.Fighter(RNG, "Bleaksword", 34, 30, 567,0,21,30,7);
			
			Fighter DodgeMaster = new Program.Fighter(RNG, "Dodger",999,0,800,0,8,0,0);
			Fighter Resister = new Program.Fighter(RNG, "Resister", 0,0,800,(double)0.5,0,0,8);
			// random, name,  attack,  defense,  hp,  resist,  damage,  armor,  apDamage
			
			DeathRunner.GiveBonus(0,8);
			StormVermin.GiveBonus(0,8);
			StormVermin2.GiveBonus(0,8);
			StormVerminHal.GiveBonus(0,8);
			PlagueMonk.GiveBonus(0,8);
			PlagueMonk.DamageMult=(double)1.12; //frenzy
			
			Swordmaster.GiveBonus(2,12);
			PhoenixGuard.GiveBonus(2,12);
			
			Console.ReadKey(true);
			double wincount = 0;
			for(int i=0;i<10000;i++)
			{
				//ChaosWarrior._BaseDefense = ChaosWarrior._BaseDefense*.6;
				//ChaosWarrior.Exhausted = true;
				//StormVermin.Exhausted = true;
				bool count = RunFight(DeathRunner,IronBreaker);
				if(count){wincount++;}
			}
			Console.WriteLine(wincount / 10000);
			Console.ReadKey(true);
			
			//RunFight(StormVermin, ChaosWarrior);
			Console.ReadKey(true);
			
		}
		
		public class Damage
		{
			public double NonAP;
			public double AP;
			public Damage(double regular, double ap)
			{
				NonAP = regular; AP = ap;
			}
		}
		
		public class Fighter
		{
			const double MAXHIT = (double).9;
			const double MINHIT = (double).08;
			public string Name;
			public double Attack
			{
				get{
					double result = _BaseAttack;
					if(ApplyBonusIfOverHalfHP && HP > (_StartingHP*(double)0.5))
					{
						result = result + BonusAttack;
					}
					if(Exhausted){result=result*EXH_ATTACKMULT;}
					return result;
				}
				set{_BaseAttack=value;}
			}
			public double Defense
			{
				get{
					double result = _BaseDefense;
					if(ApplyBonusIfOverHalfHP && HP > (_StartingHP*(double)0.5))
					{
						 result = result + BonusDefense;
					}
					if(Exhausted){result = result * EXH_DEFENSEMULT;}
					return result;
				}
				set{_BaseDefense=value;}
			}
			public double HP;
			private double _StartingHP;
			public double Resist;
			private double _Damage;
			private double _APDamage;
			
			public double Damage
			{
				get{ return _Damage * DamageMult;}
				set{ _Damage = value;}
			}
			public double APDamage
			{
				get{
					if(Exhausted){return _APDamage*DamageMult*EXH_APDAMAGEMULT;}
					return _APDamage * DamageMult;
				}
				set{_APDamage = value;}
			}
			
			public double Armor
			{
				get
				{
					if(Exhausted){return _Armor*EXH_ARMORMULT;}
					return _Armor;
				}
				set
				{
					_Armor = value;
				}
			}
			double _Armor;
			public bool Poison;
			public bool Sunder;
			Random rng;
			
			public bool Exhausted = false;
			const double EXH_ATTACKMULT = .7;
			const double EXH_ARMORMULT = .75;
			const double EXH_APDAMAGEMULT = .9;
			const double EXH_DEFENSEMULT = .9;
			
			public double BonusAttack=0;
			public double BonusDefense=0;
			
			public double DamageMult = 1;
			
			public bool ApplyBonusIfOverHalfHP=false;
			
			public double _BaseAttack;
			public double _BaseDefense;
			
			public Fighter(Random random, string name, double attack, double defense, double hp, double resist, double damage, double armor, double apDamage, bool poison, bool sunder)
			{
				rng = random;
				Name = name;
				Attack = attack;
				Defense = defense;
				HP = hp;
				Resist = resist;
				Damage = damage;
				_StartingHP = hp;
				Armor = armor;
				APDamage = apDamage;
				Poison = poison;
				Sunder = sunder;
			}
			
			public void GiveBonus(double attack, double defense)
			{
				ApplyBonusIfOverHalfHP = true;
				BonusAttack = attack;
				BonusDefense = defense;
			}
			
			public Fighter(Random random, string name, double attack, double defense, double hp, double resist, double damage, double armor, double apDamage)
			{
				rng = random;
				Name = name;
				Attack = attack;
				Defense = defense;
				HP = hp;
				Resist = resist;
				Damage = damage;
				_StartingHP = hp;
				Armor = armor;
				APDamage = apDamage;
				Poison = false;
				Sunder = false;
			}
			
			public void BeAttacked(double attackscore, double attackDamage, double attackAPDamage, bool attacksunder)
			{
				double ChanceToBeHit = (double)(35 + (attackscore - Defense))/100;
				if(ChanceToBeHit > MAXHIT) { ChanceToBeHit = MAXHIT;}
				if(ChanceToBeHit < MINHIT) { ChanceToBeHit = MINHIT;}
				bool Hit = ((double)rng.NextDouble() < ChanceToBeHit) ? true : false;
				if(Hit)
				{
					double DamageTaken = attackDamage;
					if(Poison)
					{
						DamageTaken = DamageTaken * (double)0.8; // reduce attacker damage via poison
					}
					
					double ArmorReduction = ArmorBonusOne(this.Armor, attacksunder);
					DamageTaken = DamageTaken * (1-Resist) * (1-ArmorReduction); //armor functions the same as resist to non ap damage
					if(Poison)
					{
						DamageTaken = DamageTaken + (attackAPDamage * (1-Resist) * (double)0.8); //poison reduces AP damage too
					}
					else{DamageTaken = DamageTaken + (attackAPDamage * (1-Resist));} //resistances reduce ap damage too
					HP=HP-DamageTaken;
				}
			}
			
			public void FullHeal()
			{
				HP = _StartingHP;
			}
			
			public double ArmorBonusOne(double armor, bool sunder)
			{
				//Value is selected between Armor and Armor/2, then capped at 1
					double RandomDouble = (double)(rng.NextDouble()*.5 + .5);
					double ArmorReduction;
					if(sunder){ ArmorReduction = (armor/200) * RandomDouble;} //sundered armor counts only as half its value
					else{ ArmorReduction = (armor/100) * RandomDouble;}
					if(ArmorReduction > 1){ArmorReduction=1;} //having really good armor shouldnt heal you!
					return ArmorReduction;
			}
			
			public double ArmorBonusTwo(double armor, bool sunder)
			{
				//Value is selected between Min(Armor,100) and Armor/2
				double CappedArmor = armor;
				double HalfArmor = armor/2;
				if(sunder){CappedArmor = armor/2; HalfArmor=armor/4;}
				if(CappedArmor > 100){CappedArmor = 100;}
				double ArmorReduction = (double)rng.Next((int)HalfArmor, (int)Armor+1)/100;
					if(ArmorReduction > 1){ArmorReduction=1;} //having really good armor shouldnt heal you!
					return ArmorReduction;
			}
		}
		
		public static bool RunFight(Fighter fighter1, Fighter fighter2) //return true if 1 wins
		{
			bool FIGHT = true;
			Random roll = new Random();
			fighter1.FullHeal();
			fighter2.FullHeal();
			while(FIGHT)
			{
				bool OneHitsFirst = (roll.NextDouble() >.5)?true:false;
				if(OneHitsFirst)
				{
					fighter2.BeAttacked(fighter1.Attack, fighter1.Damage, fighter1.APDamage, fighter1.Sunder);
					if(fighter2.HP <= 0)
					{
						FIGHT = false;
						break;
					}
					fighter1.BeAttacked(fighter2.Attack, fighter2.Damage, fighter2.APDamage, fighter2.Sunder);
					if(fighter1.HP <= 0)
					{
						FIGHT = false;
					}
					continue;
				}
				fighter1.BeAttacked(fighter2.Attack, fighter2.Damage, fighter2.APDamage, fighter2.Sunder);
				if(fighter1.HP <= 0)
				{
					FIGHT = false;
					break;
				}
				fighter2.BeAttacked(fighter1.Attack, fighter1.Damage, fighter1.APDamage, fighter1.Sunder);
				if(fighter2.HP <= 0)
				{
					FIGHT = false;
				}
				//Console.WriteLine("{0}:{1}   {2}:{3}", fighter1.Name, fighter1.HP, fighter2.Name, fighter2.HP);
			}
			Console.WriteLine("{0}:{1}   {2}:{3}", fighter1.Name, Math.Truncate(fighter1.HP), fighter2.Name, Math.Truncate(fighter2.HP));
			if(fighter2.HP <= 0)
			{
				return true;
			}
			return false;
		}
	}
}