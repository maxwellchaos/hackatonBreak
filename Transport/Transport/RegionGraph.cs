using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Transport
{
    class RegionGraph
    {
        public int sumTime;
        public int countAuto = 1;
        public Collection<Auto> autos = new Collection<Auto>();
        Random rnd = new Random();
        //структура графа 
        public Dictionary<int, Node> Nodes = new Dictionary<int, Node>();

        public void moveAllAuto()
        {
            foreach (Auto a in autos)
            {
                a.MoveAuto();
                if (a.Point == a.rr.Length)
                {
                    if (Nodes[a.rr.outNode].NodeType == 3)
                    {
                        //дестрой
                        // autos.Remove(a);
                        // break;
                    }
                    else
                    {
                        //Выбрать случайную дорогу из точки входа
                        int r = rnd.Next(Nodes[a.rr.outNode].outRoads.Count);
                        //Передвинуть авто
                        a.newRoad(Nodes[a.rr.outNode].outRoads[r]);
                    }
                }
            }

            //Чтоб не тормозило
            foreach (Auto a in autos)
            {
                a.MoveAuto();
                if (a.Point == a.rr.Length)
                {
                    if (Nodes[a.rr.outNode].NodeType == 3)
                    {
                        //дестрой
                        sumTime += a.AllPoint;
                        countAuto++;
                        autos.Remove(a);
                        break;

                    }
                }
            }
        }

        public void AddNode(int id, int NodeType, int x, int y)
        {
            Nodes.Add(id, new Node(NodeType, x, y));
        }
        public void AddRoad(int startNode, int finishNode, int length = 10, int RoadCount = 1)
        {
            Road r = new Road();
            r.inNode = startNode;
            r.outNode = finishNode;
            r.Length = length;
            r.RoadCount = RoadCount;
            Nodes[startNode].outRoads.Add(r);
        }

        public void GenerateAuto()
        {
            //Найти входную Node
            foreach (KeyValuePair<int, Node> kvp in Nodes)
            {
                //Ищем входы в район
                if (kvp.Value.NodeType == 2)
                {
                    //в 20% случаев запускаем новую машинку
                    int num = rnd.Next(10);
                    if (num > 2)
                    {
                        if (kvp.Value.outRoads.Count != 0)
                        {
                            //Выбрать случайную дорогу из точки входа
                            int r = rnd.Next(kvp.Value.outRoads.Count);
                            //Сгенерить авто
                            Auto a = new Auto(kvp.Value.outRoads[r]);
                            autos.Add(a);
                        }
                    }
                }
            }
        }
    }

    class Auto //машинка
    {
        public int Point; //отрезок на ребре
        public int AllPoint; //кол-во отрезков на всех ребрах, которых проехала машинка (путь)
        public Road rr;//Дорога, по которой едем.
        public int xpoint;
        public int ypoint;

        public Auto(Road rr)
        {
            Point = 0;
            AllPoint = 0;
            this.rr = rr;
        }



        public void MoveAuto()
        {
            if (Point < rr.Length)
            {
                Point++;
                AllPoint++;
            }
        }
        public void newRoad(Road r)
        {
            rr = r;
            AllPoint++;
            Point = 0;
        }
    }


    class Road
    {
        public int inNode;//id вершины куда входим 
        public int outNode;//id вершины откуда выходим ???? 
        public int Length = 10;//Длина дороги 
        public int RoadCount = 1;//Кол-во полос 
    }

    class Node
    {
        public int x;
        public int y;
        public Node(int NType, int x, int y)
        {
            NodeType = NType;
            this.x = x;
            this.y = y;
        }

        public int NodeType; //1 - перекресток 
                             //2 - вход в район из города 
                             //3 - Въезд в район со стоянки у домов 
                             //Исходяшие вершины 
        public Collection<Road> outRoads = new Collection<Road>();

    }
}
