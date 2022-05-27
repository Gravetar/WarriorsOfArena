/*
���� ���������� ����� ������ ���������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/
public class DamageTankNode : Node
{
    private Ai _ai;// ������� ��
    private ActionPlayer _actionPlayer;// �������������� �������� ������
    private float _fightTime; // ����� ���
    private int _favoriteIdWeaponPlayer;// �������������� ������ ������

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="ai">������� ��</param>
    /// <param name="actionPlayer">�������������� �������� ������</param>
    /// <param name="favoriteIdWeaponPlayer">�������������� ������ ������</param>
    /// <param name="fightTime">����� ���</param>
    public DamageTankNode(Ai ai, ActionPlayer actionPlayer, float fightTime, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _fightTime = fightTime;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <returns>��������� ����</returns>
    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Block && _fightTime > 60 && _favoriteIdWeaponPlayer == 0) // ���� ����� ����������� ����������� � ��� �������������� ������ - ��� � ����� ��� ���� ������ ������
        {
            _ai.SetWeapon(2);// ���������� ������ ����� - ������

            //���������� ������������ ���� � ��������
            _ai.SetStrength(3);
            _ai.SetDexterity(1);

            _ai.SetTactic(TacticEnemy.Aggresive);//���������� ������� �����������
            return NodeState.SOCCESS;// �������� ���������� ����
        }
        else
        {
            return NodeState.FAILURE;// ������ ���������� ����
        }
    }
}
