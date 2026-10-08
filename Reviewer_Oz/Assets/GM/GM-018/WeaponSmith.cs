using UnityEngine;
using System.Collections.Generic;

namespace GM_018
{
    

    public class WeaponSmith : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) Copy();
            if (Input.GetKeyDown(KeyCode.W)) Share();
            if (Input.GetKeyDown(KeyCode.E)) ClearMemories();
            if (Input.GetKeyDown(KeyCode.R)) TestScenario();
        }

        //1번
        struct NormalWeapon
        {
            string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            int power;
            public int Power
            {
                get { return power; }
                set { power = value; }
            }

            public NormalWeapon(string name, int power)
            {
                this.name = name;
                this.power = power;
            }
        }
        class LegendaryWeapon
        {
            string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            int power;
            public int Power
            {
                get { return power; }
                set { power = value; }
            }

            public LegendaryWeapon(string name, int power)
            {
                this.name = name;
                this.power = power;
            }
        }

        LegendaryWeapon original = new LegendaryWeapon("오리지널", 100);
        LegendaryWeapon linked;
        private void Start()
        {
            linked = original;
        }
        void Copy()
        {
            NormalWeapon original = new NormalWeapon("오리지널", 10);
            NormalWeapon copy = original;

            copy.Name = "카피";
            copy.Power = 50; ;

            Debug.Log($"원본 무기 스펙 : {original.Name} - 공격력 {original.Power}");
            Debug.Log($"복사 무기 스펙 : {copy.Name} - 공격력 {copy.Power}");

        }

        void Share()
        {
            linked.Name = "카피";
            linked.Power = 999;

            Debug.Log($"원본 무기 스펙 : {original.Name}  - 공격력  {original.Power}");
            Debug.Log($"복사 무기 스펙 : {linked.Name}  - 공격력  {linked.Power}");
        }

        void ClearMemories()
        {
            linked = null;
            Debug.Log($"메모리 청소 시뮬레이션입니다.");
            if (original == null)
            {
                Debug.Log("원본 삭제됨.");
                return;
            }
            Debug.Log($"원본 무기 스펙 : {original.Name}  - 공격력  {original.Power}");
            if (linked == null)
            {
                Debug.Log("복사본 삭제됨");
                return;
            }
            Debug.Log($"복사 무기 스펙 : {linked.Name}  - 공격력  {linked.Power}");
            Debug.Log($"-------------------------------------------------");
        }



        //2번
        struct BaseStats
        {
            public string name;
            public int hp;
            public int atk;

            public BaseStats(string name, int hp, int atk)
            {
                this.name = name;
                this.hp = hp;
                this.atk = atk;
            }
        }
        class SkillSet
        {
            public string skillName;
            public float multiplier;

            public SkillSet(string skillName, float multiplier)
            {
                this.skillName = skillName;
                this.multiplier = multiplier;
            }
            public SkillSet CopySkill()
            {
                SkillSet copy = new SkillSet(skillName, multiplier);
                return copy;
            }
        }

        class MonsterTemplate
        {
            public BaseStats stats;
            public List<SkillSet> skills = new List<SkillSet>();

            public MonsterTemplate(BaseStats stats, List<SkillSet> skills)
            {
                this.stats = stats;
                this.skills = skills;
            }

            public MonsterTemplate Clone()
            {
                List<SkillSet> cloneSkills = new List<SkillSet>();
                foreach (SkillSet s in skills)
                {
                    cloneSkills.Add(s.CopySkill());
                }
                MonsterTemplate clone = new MonsterTemplate(stats, cloneSkills);
                return clone;
            }
            public void AddSkills(SkillSet newskill)
            {
                skills.Add(newskill);
            }
        }

        void TestScenario()
        {
            MonsterTemplate dragon = new MonsterTemplate(new BaseStats("드래곤", 1000, 300), new List<SkillSet>());
            dragon.AddSkills(new SkillSet("화염방사", 1.5f));

            MonsterTemplate dragon_inst1 = dragon.Clone();

            dragon_inst1.stats.hp = 500;
            dragon_inst1.skills[0].multiplier = 9.9f;

            Debug.Log($"원본 드래곤 - {dragon.stats.name} : HP {dragon.stats.hp} | ATK {dragon.stats.atk}");
            for (int i = 0; i < dragon.skills.Count; i++)
                Debug.Log($"Skill {i + 1} {dragon.skills[i].skillName} | 계수 {dragon.skills[i].multiplier}");
            Debug.Log($"복사 드래곤 - {dragon_inst1.stats.name} : HP {dragon_inst1.stats.hp} | ATK {dragon_inst1.stats.atk}");
            for (int i = 0; i < dragon_inst1.skills.Count; i++)
                Debug.Log($"Skill {i + 1} {dragon_inst1.skills[i].skillName} | 계수 {dragon_inst1.skills[i].multiplier}");
        }
    }
}


