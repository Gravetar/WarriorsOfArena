/*
Основной класс машины состояний - конечных автоматов

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

public class StateMachine
{
    /// <summary>
    /// Текущее состояние
    /// </summary>
    public State CurrentState;

    /// <summary>
    /// Инициализация конечных автоматов
    /// </summary>
    /// <param name="startingState">Начальное состояние</param>
    public void Initialize (State startingState)
    {
        CurrentState = startingState; // Установить в качестве текущего состояния - стартовое 
        startingState.Enter(); // Войти в состояние
    }

    /// <summary>
    /// Ежекадровое обновление - действия в состоянии
    /// </summary>
    public void LogicUpdate()
    {
        CurrentState?.LogicUpdate();
    }

    /// <summary>
    /// Сменить состояние
    /// </summary>
    /// <param name="newState">Новое состояние</param>
    public void ChangeState(State newState)
    {
        CurrentState.Exit(); // Выйти из текущего состояния

        CurrentState = newState; // Установить в качестве текущего состояния - новое
        newState.Enter(); // Войти в новое состояние
    }
}
