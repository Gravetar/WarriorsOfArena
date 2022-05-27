/*
���� ����� ������ ���������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

public class WarriorNode : Node
{
    private Ai _ai; // ������� ��
    private ActionPlayer _actionPlayer; // �������������� �������� ������
    private int _favoriteIdWeaponPlayer; // �������������� ������ ������

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="ai">������� ��</param>
    /// <param name="actionPlayer">�������������� �������� ������</param>
    /// <param name="favoriteIdWeaponPlayer">�������������� ������ ������</param>
    public WarriorNode(Ai ai, ActionPlayer actionPlayer, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <returns>��������� ����</returns>
    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Attack && _favoriteIdWeaponPlayer == 0) // ���� ����� ����������� ��������� � ��� �������������� ������ - ���
        {
            _ai.SetWeapon(2); // ���������� ������ �����

            //���������� ������������ ���� � �������� 
            _ai.SetStrength(2);
            _ai.SetDexterity(2);

            _ai.EnemyTactic = TacticEnemy.Passive;//���������� ������� ���������
            return NodeState.SOCCESS; // �������� ���������� ����
        }
        else
        {
            return NodeState.FAILURE; // ������ ���������� ����
        }
    }
}
